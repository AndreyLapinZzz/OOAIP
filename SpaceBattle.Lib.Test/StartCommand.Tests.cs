using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class StartCommandTests
{
    public StartCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockcmd = new Mock<ICommand>();
        mockcmd.Setup(c => c.execute());

        var mockStrategyReturnsCmd = new Mock<IStrategy>();
        mockStrategyReturnsCmd.Setup(sc => sc.RunStrategy(It.IsAny<object[]>())).Returns(mockcmd.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SetProperty", (object[] args) => mockStrategyReturnsCmd.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) => mockStrategyReturnsCmd.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Push", (object[] args) => mockStrategyReturnsCmd.Object.RunStrategy(args)).Execute();

    }

    [Fact]
    public void SuccessofStartCommandExecute()
    {
        var mcs = new Mock<IStartable>();
        mcs.SetupGet(c => c.uobject).Returns(new Mock<IUObject>().Object).Verifiable();
        mcs.SetupGet(c => c.properties).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();

        ICommand smc = new StartCommand(mcs.Object);
        smc.execute();

        mcs.Verify();
    }

    [Fact]
    public void StartCommandThrowsExceptionCantToGetUObjectNegativeTest()
    {
        var mcs = new Mock<IStartable>();
        mcs.SetupGet(c => c.uobject).Throws<Exception>().Verifiable();
        mcs.SetupGet(c => c.properties).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(It.IsAny<int>(), It.IsAny<int>()) } }).Verifiable();

        ICommand smc = new StartCommand(mcs.Object);

        Assert.Throws<Exception>(() => smc.execute());
    }

    [Fact]
    public void StartCommandThrowsExceptionCantToGetValueOfVelocityNegativeTest()
    {
        var mcs = new Mock<IStartable>();
        mcs.SetupGet(a => a.uobject).Returns(new Mock<IUObject>().Object).Verifiable();
        mcs.SetupGet(a => a.properties).Throws<Exception>().Verifiable();

        ICommand smc = new StartCommand(mcs.Object);

        Assert.Throws<Exception>(() => smc.execute());
    }

}
