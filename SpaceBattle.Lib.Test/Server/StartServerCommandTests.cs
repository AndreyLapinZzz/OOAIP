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
    }

    [Fact] 
    public void ZeroLenTest()
    {
        var mockIds = new List<uint>().AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

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

        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Never());
        mockStartCommand.Verify(cmd => cmd.execute(), Times.Never());
    }
    
    [Fact]
    public void PositivLenTest()
    {
        uint serverCount = 3;

        var mockIds = new List<uint>{ 1, 2, 3 }.AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

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

        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly((int)serverCount));
        mockStartCommand.Verify(cmd => cmd.execute(), Times.Exactly((int)serverCount));
    }
    
    
    [Fact]
    public void GenerateThreadIdsNotInIoCTest()
    {
        uint serverCount = 3;
        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
    }
    
    [Fact]
    public void NotIEnumerableGenerateThreadIdsTest()
    {
        uint serverCount = 1;
        int Any = -3;

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

        StartServerCommand cmd = new StartServerCommand(serverCount);

        var act = () => cmd.execute();
        
        Assert.Throws<InvalidCastException>(act);
    }
    
    [Fact]
    public void ErrorInGenerateThreadIdsTest()
    {
        uint serverCount = 1;

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

        StartServerCommand cmd = new StartServerCommand(serverCount);

        var act = () => cmd.execute();

        Assert.Throws<InvalidOperationException>(act);
    }
    
    [Fact]
    public void StartStrategyNotInIoCTest()
    {
        uint serverCount = 3;

        var mockIds = new List<uint> { 1, 2, 3 }.AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

        StartServerCommand cmd = new StartServerCommand(serverCount);
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void NotICommandStartStrategyTest()
    {
        uint serverCount = 1;

        var mockIds = new List<uint> { 1 }.AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

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
        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ErrorInStartCommandTest()
    {
        uint serverCount = 1;

        var mockIds = new List<uint> { 1 }.AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

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

        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockStartTheadsStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ErrorInStartStrategyTest()
    {
        uint serverCount = 1;

        var mockIds = new List<uint> { 1 }.AsEnumerable();

        Mock<IStrategy> mockGenerateThreadIds = new();
        mockGenerateThreadIds.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockIds).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GenerateThreadIds",
            (object[] args) => mockGenerateThreadIds.Object.RunStrategy(args)
        ).Execute();

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
        mockGenerateThreadIds.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
}
