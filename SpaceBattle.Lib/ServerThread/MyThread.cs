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
            IoC.Resolve<ICommand>("Game.ExceptionHandler", new Exception(), cmd.GetType());
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
    }
}
