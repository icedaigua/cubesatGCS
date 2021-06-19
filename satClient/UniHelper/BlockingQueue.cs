using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UniHelper
{
    public class BlockingQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly int maxSize;

        public BlockingQueue(int maxSize)
        {
            this.maxSize = maxSize;
        }

        public bool IsEmpty()
        {
            lock (queue)
            {
                return queue.Count == 0;
            }
        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)  //队列若满,则清除最早的数据
                {
                    //Monitor.Wait(queue);
                    Trace.WriteLine("队列已满,开始清楚最早的数据");
                    Dequeue();
                }
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    Monitor.PulseAll(queue);  //wake up any blocked dequeue
                }
            }
        }

        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)  //队列为空, 返回null
                {
                    //Monitor.Wait(queue);
                    return default(T);
                }
                T item = queue.Dequeue();
                if (queue.Count == maxSize - 1)
                {
                    Monitor.PulseAll(queue);  //wake up any blocked enqueue
                }

                return item;
            }
        }

    }
}
