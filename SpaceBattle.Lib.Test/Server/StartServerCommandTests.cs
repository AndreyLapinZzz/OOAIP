using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System;

namespace SpaceBattle.Lib.Test;

public class StartServerCommandTests{
    public StartServerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    [Fact] // OK
    public void ZeroLenTest()
    {
        Mock<ICommand> mockStartCommand = new();
        mockStartCommand.Setup(
            cmd => cmd.execute()
        ).Verifiable();
        Mock<IStrategy> mockStartTheadsStrategy = new();
        mockStartTheadsStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockStartCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartTheadsStrategy",
            (object[] args) => mockStartTheadsStrategy.Object.RunStrategy(args)
        ).Execute();
        StartServerCommand cmd = new StartServerCommand(0);
        cmd.execute();
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Never());
        mockStartCommand.Verify(cmd => cmd.execute(), Times.Never());
    }

    [Fact]
    public void PositivLenTest()
    {
        uint serverCount = 3;
        int serverCount2 = 3;
        Mock<ICommand> mockStartCommand = new();
        mockStartCommand.Setup(
            strategy => strategy.execute()
        ).Verifiable();
        Mock<IStrategy> mockStartTheadsStrategy = new();
        mockStartTheadsStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockStartCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartTheadsStrategy",
            (object[] args) => mockStartTheadsStrategy.Object.RunStrategy(args)
        ).Execute();
        StartServerCommand cmd = new StartServerCommand(serverCount);
        cmd.execute();
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(serverCount2));
        mockStartCommand.Verify(cmd => cmd.execute(), Times.Exactly(serverCount2));
    }

    [Fact]
    public void StartStrategyNotInIoCTest()
    {
        uint serverCount = 3;
        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void NotICommandTest()
    {
        uint serverCount = 1;
        int Any = -3;
        Mock<IStrategy> mockStartTheadsStrategy = new();
        mockStartTheadsStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartTheadsStrategy",
            (object[] args) => mockStartTheadsStrategy.Object.RunStrategy(args)
        ).Execute();
        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);
    }

    [Fact]
    public void ErrorInStartCommandTest()
    {
        uint serverCount = 1;
        Mock<ICommand> mockStartCommand = new();
        mockStartCommand.Setup(
            cmd => cmd.execute()
        ).Throws<InvalidOperationException>().Verifiable();
        Mock<IStrategy> mockStartTheadsStrategy = new();
        mockStartTheadsStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockStartCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartTheadsStrategy",
            (object[] args) => mockStartTheadsStrategy.Object.RunStrategy(args)
        ).Execute();
        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ErrorInStartStrategyTest()
    {
        uint serverCount = 1;
        Mock<IStrategy> mockStartTheadsStrategy = new();
        mockStartTheadsStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartTheadsStrategy",
            (object[] args) => mockStartTheadsStrategy.Object.RunStrategy(args)
        ).Execute();
        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
    }
}
