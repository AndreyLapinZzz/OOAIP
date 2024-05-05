using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;
public class MessageHandlerTests
{
    public MessageHandlerTests()
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

    [Fact]
    public void InterpretationCommandTest()
    {
        var getGameObj_Strat = new Mock<IStrategy>();
        var obj = new Mock<IUObject>();
        getGameObj_Strat.Setup(m=>m.RunStrategy("Item548")).Returns(obj.Object);
        var Move_Strat = new Mock<IStrategy>();
        var icom = new Mock<ICommand>();
        bool command_got = false;
        Move_Strat.Setup(m=>m.RunStrategy(obj.Object)).Returns(icom.Object).Callback(()=>{command_got = true;});
        var getQueue_Strat = new Mock<IStrategy>();
        var queue = new Queue<ICommand>();
        getQueue_Strat.Setup(m=>m.RunStrategy("Game123")).Returns(queue);
        var mockcmd = new Mock<ICommand>();
        mockcmd.Setup(c => c.execute());
        var setProp_Strat = new Mock<IStrategy>();
        setProp_Strat.Setup(sc => sc.RunStrategy(It.IsAny<object[]>())).Returns(mockcmd.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameQueue.PushCommand", (object[]args)=>new GameQueuePushCommandStrategy().RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ConstructCommand", (object[]args)=>new ConstructCommandStrategy().RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameObject", (object[]args)=>getGameObj_Strat.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SetProperty", (object[]args) => setProp_Strat.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[]args)=>Move_Strat.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue",(object[]args)=>getQueue_Strat.Object.RunStrategy(args)).Execute();

        var m = new Mock<IMessage>();
        var prop = new Dictionary<string,object>(){{"Velocity",2}};
        m.SetupGet(m=>m.CommandName).Returns("Move");
        m.SetupGet(m=>m.GameId).Returns("Game123");
        m.SetupGet(m=>m.GameItemId).Returns("Item548");
        m.SetupGet(m=>m.CommandParams).Returns(prop);
        var cmd = new InterpretationCommand(m.Object);
        cmd.execute();
        Assert.True(command_got);
    }
}
