namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread thread = (MyThread) args[0];
        
        thread.Stop();
        
        return 0;
    }
}