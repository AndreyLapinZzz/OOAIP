using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;
public interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
    void Push(ICommand cmd);
}

public class MyThread
{
    public Thread thread;
    public IReceiver queue;
    public Action strategy;
    public bool stop = false;

    internal void Stop()
    {
        stop = true;
    }

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        try
        {
            cmd?.execute();
        }
        catch
        {
            // throw new Exception();
            // throw new Exception();
            // IoC.Resolve<ICommand>("Game.ExceptionHandler", new Exception(), cmd);
        }
    }
    public MyThread(IReceiver receiver)
    {
        this.queue = receiver;
        this.strategy = () =>
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
        this.strategy = newBehaviour;
    }
    public void execute()
    {
        thread.Start();
        //IoC.Resolve<ICommand>("CreateAndStartThread", thread, strategy);
    }
    // internal IReceiver GetQueue()
    // {
    //     return queue;
    // }
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

public class CreateAndStartThreadCommand : ICommand
{
    MyThread thread;
    public CreateAndStartThreadCommand(MyThread thread) => this.thread = thread;
    public void execute()
    {
        thread.execute();
    }
}

public class SendCommand : ICommand
{
    MyThread thread;
    ICommand cmd;

    public SendCommand(MyThread thread, ICommand cmd)
    {
        this.thread = thread;
        this.cmd = cmd;
    }

    public void execute()
    {
        //ICommand command = IoC.Resolve<ICommand>("Game.SendCommand", thread, cmd);
        thread.queue.Push(cmd);
    }
}

public class HardStopCommand : ICommand
{
    MyThread stoppingThread;
    public HardStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        // if (Equals(stoppingThread.thread, Thread.CurrentThread))
        // {
        //     stoppingThread.Stop();
        // }
        // else
        // {
        //     throw new Exception();
        // }
        try
        {
            stoppingThread.Stop();
        }
        catch
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
        // if (Equals(stoppingThread, Thread.CurrentThread))
        // {
        try
        {
            //IoC.Resolve<ICommand>("Game.SoftStopTheThread", stoppingThread);
            IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.HardStopThreadStrategy", stoppingThread)).execute();
        }
        catch
        {
            throw new Exception();
        }
        // else
        // {
        //     throw new Exception();
        // }
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
    
    public void Push(ICommand cmd)
    {
        queue.Add(cmd);
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


// public class WhatTheIntrestingThings {

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
// }