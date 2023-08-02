namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        int threadId = (int)args[0];

        IReceiver reciever = (IReceiver)args[1];

        MyThread thread = new MyThread(threadId, reciever);
        
        return thread;
    }
}