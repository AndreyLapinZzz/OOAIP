using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;
public interface IReceiver
{
    ICommand Receive();
}

public class MyThread
{
    Thread thread;
    IReceiver queue;
    Action? strategy;

    bool stop = false;
    public void HardStop()
    {
        IoC.Resolve<ICommand>("Game.HardStopTheThread", thread);
        // queue.Receive();
        // stop = true;
    }

    public void SoftStop()
    {
        IoC.Resolve<ICommand>("Game.SoftStopTheThread", thread);
        // while (true)
        // {
        //     HandleCommand();
        // }

        // this.HardStop();
        // // queue.Receive();
        // // ждём пока какие-то команды выполнятся
        // // stop = true;
        
    }

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        try
        {
        cmd.execute();
        }
        catch
        {
            IoC.Resolve<ICommand>("Game.ExceptionHandler", new Exception(), cmd);
        }
    }
    public MyThread(IReceiver receiver)
    {
        this.queue = receiver;
        Action strategy = () =>
        {
            HandleCommand();
        };

        Thread thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy();
            }
        });
        this.thread = thread;
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;
    }
    public void execute()
    {
        thread.Start();
    }
}

public class UpdateBehaviourCommand : ICommand
{
    Action newBehaviour;
    MyThread thread;

    public UpdateBehaviourCommand(MyThread thread, Action newBehaviour)
    {
        this.thread = thread;
        this.newBehaviour = newBehaviour;
    }

    public void execute()
    {
        thread.UpdateBehaviour(newBehaviour);
    }
}


// class ThreadStopCommand : ICommand
// {
//     MyThread stoppingThread;
//     public ThreadStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
//     public void execute()
//     {
//         if (Thread.CurrentThread == stoppingThread)
//         {
//             stoppingThread.Stop();
//         }
//         else
//         {
//             throw new Exception();
//         }
//     }
// }

public class ReceiveAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;
    public ReceiveAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.LongCount() == 0;
    }
}

// class GameCommand : ICommand
// {
//     IQueue<ICommand> queue = new Queue<ICommand>(40);

//     object scope;

//     public void execute()
//     {
//         IoC.Resolve<ICommand>("Scope.Current.Set", scope).execute();

//         while(true)//время выполнения игры меньше выделенного кванта
//         {
//             //время начала операции
//             queue.Take().execute();
//             //время завершения операции
//             //увеличить время выполнения игры
//         }
//     }
// }

// BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>(1000);

// ReceiveAdapter receiver = new ReceiveAdapter(queue);

// MyThread thread = new MyThread(receiver);

// thread.execute();

// queue.Add(new ThreadStopCommand(
//     thread,
//     () =>
//     {
//         if(queue.isEmpty)
//         {
//             thread.Stop();
//         }
//         else
//         {
//             thread.HandleCommand();
//         }
//     }
// ));