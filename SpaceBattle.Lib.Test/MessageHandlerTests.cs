using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.test;
public class GameQueuePushTests
{
    public GameQueuePushTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void GameQueuePushCommandStrategyTest()
    {
        var icom = new Mock<ICommand>();
        var queue = new Mock<Queue<ICommand>>();
        var getQueue_Strat = new Mock<IStrategy>();
        getQueue_Strat.Setup(m=>m.RunStrategy("Game123")).Returns(queue.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue",(object[]args)=>getQueue_Strat.Object.RunStrategy(args)).Execute();

        var strat = new GameQueuePushCommandStrategy();
        var cmd =(ICommand) strat.RunStrategy("Game123", icom.Object);
        cmd.execute();
        Assert.Single(queue.Object);
    }
}
