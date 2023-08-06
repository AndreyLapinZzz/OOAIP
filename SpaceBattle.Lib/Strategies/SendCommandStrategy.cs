namespace SpaceBattle.Lib;
using Hwdtech;

public class SendCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread thread = (MyThread)args[0];
        
        ICommand cmd = (ICommand)args[1];

        IReceiver queue = thread.GetQueue(); //??

        queue.Push(cmd);

        return queue;
    }
}