namespace SpaceBattle.Lib;
using Hwdtech;

public class SendCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        int threadId = (int)args[0];
        
        ICommand cmd = (ICommand)args[1];

        return 0;
    }
}