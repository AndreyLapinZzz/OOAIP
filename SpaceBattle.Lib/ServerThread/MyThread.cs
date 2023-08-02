using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;
public interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
}

public class MyThread
{
    int threadId;
    Thread thread;
    IReceiver queue;
    Action? strategy;
    bool stop = false;

    internal void Stop()
    {
        stop = true;
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
    public MyThread(int threadId, IReceiver receiver)
    {
        this.threadId = threadId;
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

public class SendCommand : ICommand
{
    int threadId;
    ICommand cmd;

    public SendCommand(int threadId, ICommand cmd)
    {
        this.threadId = threadId;
        this.cmd = cmd;
    }

    public void execute()
    {
        IoC.Resolve<ICommand>("Game.SendCommand", threadId, cmd);
    }
}

public class HardStopCommand : ICommand
{
    MyThread stoppingThread;
    public HardStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        if (Equals(stoppingThread, Thread.CurrentThread))
        {
            //stoppingThread.Stop();
            IoC.Resolve<ICommand>("Game.HardStopTheThread", stoppingThread);
        }
        else
        {
            throw new Exception();
        }
    }
}

public class SoftStopCommand : ICommand
{
    MyThread stoppingThread;
    public SoftStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        if (Equals(stoppingThread, Thread.CurrentThread))
        {
            IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.HardStopTheThread", stoppingThread)); // отправляем в очередь HardStop
        }
        else
        {
            throw new Exception();
        }
    }
}
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

interface ISender
{
    void Send(object message);
}


//IoC.Resolve<ICommand>("Command.SendMessage", obj).execute();



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