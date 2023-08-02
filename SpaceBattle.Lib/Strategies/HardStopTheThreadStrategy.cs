namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        string threadId = (string)args[0];
        var action = args[1];

        return action;
    }
}