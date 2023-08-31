namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread thread = (MyThread)args[0];

        ICommand createAndStartThread = new CreateAndStartThreadCommand(thread);
        
        return createAndStartThread;
    }
}
