using Thread = System.Threading.Thread;
using static System.Console;

int count = 0;
ReaderWriterLockSlim _lock = new();

void GetCount()
{
    while (true)
    {

        try
        {
            _lock.EnterReadLock();
            WriteLine($"R: Thread {Thread.CurrentThread.ManagedThreadId} is reading: {count}");
        }
        finally
        {
            _lock.ExitReadLock();
        }

        Thread.Sleep(500);
    }
}

void AddToCount(int value)
{
    while (true)
    {
        try
        {
            _lock.EnterWriteLock();
            WriteLine($"W: Thread {Thread.CurrentThread.ManagedThreadId} is writing: {count++}");
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        Thread.Sleep(2000);
    }
}

List<Thread> threadList = new();
var randomValue = new Random();
threadList.Add(new Thread(() => GetCount()));
threadList.Add(new Thread(() => AddToCount(randomValue.Next(7))));
threadList.Add(new Thread(() => GetCount()));
threadList.Add(new Thread(() => GetCount()));
threadList.Add(new Thread(() => GetCount()));
threadList.Add(new Thread(() => AddToCount(randomValue.Next(7))));
threadList.Add(new Thread(() => GetCount()));
threadList.Add(new Thread(() => GetCount()));

foreach (var thread in threadList)
{
    thread.Start();
}
