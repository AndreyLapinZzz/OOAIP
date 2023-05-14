namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        string ThreadId = (string)args[0];
        
        ICommand cmd = (ICommand)args[1];

        return cmd;
    }
}