using System;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start main. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            Worker work = new Worker();
            Task task = work.Get_2();
            task.ContinueWith((t) => { Thread.CurrentThread.IsBackground = false; Console.WriteLine("Continue \t ThreadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); Worker.Fake(); });
            Console.WriteLine("End main. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            Console.ReadKey();
        }
    }

    class Worker
    {
        public async Task Get()
        {
            Console.WriteLine("Start Get. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            await Task.Run(() => { Console.WriteLine("Start await. threadId={0} \t isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); Thread.Sleep(5000); Console.WriteLine("End await. threadId={0} \t isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); });
            Console.WriteLine("End Get. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
        }

        public async Task Get_2()
        {
            Console.WriteLine("Start Get_2. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            await Job();
            Console.WriteLine("Get_2 marker 1. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            await Task.Run(() => { Console.WriteLine("Get Task 1. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); Thread.Sleep(10000); });
            Console.WriteLine("Get_2 marker 2. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
            await Task.Run(() => { Console.WriteLine("Get Task 2. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); Thread.Sleep(3000); });

        }

        public Task Job()
        {
            return Task.Run(() => { Console.WriteLine("Start Job. \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground); Thread.Sleep(7000);});
        }

        public static void Fake()
        {
            Console.WriteLine("Fake \t threadId={0}, isBack={1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground);
        }
    }
}