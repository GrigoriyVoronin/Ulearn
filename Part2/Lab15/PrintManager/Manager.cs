using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintManager
{
    public class Manager
    {
        private ConcurrentQueue<Document> docsQueue = new ConcurrentQueue<Document>();

        private readonly ConcurrentQueue<Document> printedDocuments = new ConcurrentQueue<Document>();

        private readonly Thread main;

        private bool isWork;

        public Manager()
        {
            main = new Thread(DoWork);
            main.Start();
            isWork = true;
        }

        public void DoWork()
        {
            while (true)
            {
                if (docsQueue.Count > 0)
                {
                    if (docsQueue.TryDequeue(out var doc))
                    {
                        Thread.Sleep(doc.Type.Duration);
                        lock (printedDocuments)
                        {
                            printedDocuments.Enqueue(doc);
                        }
                    }
                }
                else
                {
                    lock (docsQueue)
                    {
                        Monitor.Wait(docsQueue);
                    }
                }

                if (!isWork)
                    break;
            }
        }

        public IEnumerable<Document> CanselPrint()
        {
            isWork = false;
            main.Join();
            foreach (var document in docsQueue)
                yield return document;
        }

        public void PrintDocument(Document document)
        {
            lock (docsQueue)
            {
                docsQueue.Enqueue(document);
                Monitor.Pulse(docsQueue);
            }
        }

        public void DeleteDocument(Document document)
        {
            Task.Run(() => DoDelete(document));
        }

        private void DoDelete(Document document)
        {
            lock (docsQueue)
            {
                var docsQueueCopy = new ConcurrentQueue<Document>();
                foreach (var documentOld in docsQueue
                    .Where(documentOld => documentOld != document))
                    docsQueueCopy.Enqueue(documentOld);
                docsQueue = docsQueueCopy;
            }
        }

        public async Task<IEnumerable<Document>> TakePrintedDocuments()
        {
            return await Task.Run(SortPrintedDocs);
        }

        private IEnumerable<Document> SortPrintedDocs()
        {
            lock (printedDocuments)
            {
                var sortedDocs = new SortedSet<Document>(new DocsComparer());
                foreach (var document in printedDocuments)
                    sortedDocs.Add(document);
                return sortedDocs;
            }
        }

        public async Task<double> AverageTime()
        {
            return await Task.Run(CalculateAverageTime);
        }

        private double CalculateAverageTime()
        {
            var time = 0.0;
            lock (printedDocuments)
            {
                time = printedDocuments
                           .Aggregate(time, (current, document) => current + document.Type.Duration) /
                       printedDocuments.Count;
            }

            return time;
        }

        private sealed class DocsComparer : IComparer<Document>
        {
            public int Compare(Document x, Document y) => x.Type.Size.CompareTo(y.Type.Size);
        }
    }
}