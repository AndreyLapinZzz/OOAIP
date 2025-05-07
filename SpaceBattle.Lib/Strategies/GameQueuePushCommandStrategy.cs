using Hwdtech;
namespace SpaceBattle.Lib;

public class GameQueuePushCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        string gameId = (string)args[0];
        ICommand cmd = (ICommand)args[1];
        var gameQueue = IoC.Resolve<Queue<ICommand>>("GetGameQueue", gameId);
        return new GameQueuePushCommand(gameQueue, cmd);
    }
}
