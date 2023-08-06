namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        IReceiver reciever = (IReceiver)args[0];

        MyThread thread = new MyThread(reciever);
        
        return thread;
    }
}
