using System.Threading;

namespace CustomThreadPoolTask.Collections
{
    public class WorkStealingQueue<T>
    {
        private const int InitialSize = 32;
        private readonly object mForeignLock = new object();
        private T[] mArray = new T[InitialSize];
        private volatile int mHeadIndex;
        private int mMask = InitialSize - 1;
        private volatile int mTailIndex;

        public override string ToString() => Count.ToString();

        public bool IsEmpty => mHeadIndex >= mTailIndex;

        public int Count => mTailIndex - mHeadIndex;

        public void LocalPush(T obj)
        {
            var tail = mTailIndex;
            if (tail < mHeadIndex + mMask)
            {
                mArray[tail & mMask] = obj; // safe! только в этом методе пишем в m_array
                mTailIndex = tail + 1; // safe! только local-операции меняют m_tailIndex
            }
            else
            {
                lock (mForeignLock)
                {
                    var head = mHeadIndex;
                    var count = mTailIndex - mHeadIndex;
                    if (count >= mMask)
                    {
                        var newArray = new T[mArray.Length << 1];
                        for (var i = 0; i < mArray.Length; i++) newArray[i] = mArray[(i + head) & mMask];
                        mArray = newArray;

                        // Reset the field values, incl. the mask.
                        mHeadIndex = 0;
                        mTailIndex = tail = count;
                        mMask = (mMask << 1) | 1;
                    }

                    mArray[tail & mMask] = obj;
                    mTailIndex = tail + 1;
                }
            }
        }

        public bool LocalPop(ref T obj)
        {
            var tail = mTailIndex;
            if (mHeadIndex >= tail) // m_headIndex может действительно уехать вперед, см. TrySteal
                return false;

            tail -= 1;
            Interlocked.Exchange(ref mTailIndex,
                tail); // Interlocked, чтобы гарантировать, что запись не произойдет позже чтения m_headIndex в следующей строчке (C# memory model)

            if (mHeadIndex <= tail)
            {
                obj = mArray[tail & mMask];
                return true;
            }

            lock (mForeignLock)
            {
                if (mHeadIndex <= tail)
                {
                    obj = mArray[tail & mMask];
                    return true;
                }

                mTailIndex = tail + 1; // проиграли гонку
                return false;
            }
        }

        public bool TrySteal(ref T obj)
        {
            var taken = false;
            try
            {
                taken = Monitor.TryEnter(mForeignLock);
                if (taken)
                {
                    var head = mHeadIndex;
                    Interlocked.Exchange(ref mHeadIndex,
                        head + 1); // Interlocked по аналогичным причинам, что и в LocalPop

                    if (head < mTailIndex)
                    {
                        obj = mArray[head & mMask];
                        return true;
                    }
                    else
                    {
                        mHeadIndex = head; // проиграли гонку
                        return false;
                    }
                }
            }
            finally
            {
                if (taken)Monitor.Exit(mForeignLock);
            }

            return false;
        }
    }
}