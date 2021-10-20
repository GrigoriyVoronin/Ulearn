public class ConcurrentQueue<T> : IConcurrentQueue<T>
{
	private Node head;
	private Node tail;
	private int count;
	
	public MyConcurrentQueue() => head = tail = new Node(default);
	
	// Важный инвариант: .Next меняется только здесь
	public void Enqueue(T value)
	{
		var node = new Node(value);

		while (true)
		{
			var localTail = tail;
			var localNext = localTail.Next;

			if (localNext == null) // Текущий tail последний и ни на кого не ссылается 
			{
				// Если tail.Next никто не обновил (а это может сделать только другой поток в Enqueue)
				// То добавялем ссылку на новый узел
				if (Interlocked.CompareExchange(ref localTail.Next, node, null) == null) 
					break;
			}
			else // Текущий tail не последний в цепочке
			{
				// Пытаемся сдвинуть указатель tail на последний узел в цепочке (один шаг)
				Interlocked.CompareExchange(ref tail, localNext, localTail);
			}
		}

		{
			var localTail = tail;
			var localNext = localTail.Next;
			// Пытаемся сдвинуть указатель tail на последний узел в цепочке (один шаг)
			Interlocked.CompareExchange(ref tail, localNext, localTail);
		}

		Interlocked.Increment(ref count);
	}

	// Важный инвариант: head меняется только здесь
	public bool TryDequeue(out T value)
	{
		value = default;
		
		while (true)
		{
			var localTail = tail;
			var localHead = head;
			var localNext = localHead.Next;

			if (localHead == localTail)
			{
				if (localNext == null)
					return false;
				
				// Пытаемся сдвинуть указатель tail на последний узел в цепочке (один шаг)
				Interlocked.CompareExchange(ref tail, localNext, localTail);
			}
			else
			{
				value = localNext.Value;
				// Сдвигаем head ближе к tail
				if (Interlocked.CompareExchange(ref head, localNext, localHead) == localHead)
				{
					Interlocked.Decrement(ref count);
					return true;
				}
			}
		}
	}

	public bool TryPeek(out T value)
	{
		var localNext = head.Next;
		
		value = localNext != null ? localNext.Value : default;
		return localNext != null;
	}

	public int Count => count;
	
	private class Node
	{
		public readonly T Value;
		public Node Next;

		public Node(T value) => Value = value;
	}
}