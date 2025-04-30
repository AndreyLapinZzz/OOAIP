using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SpaceBattle.Lib.Test;

public class StopServerCommandTests
{
    public StopServerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void ThreadsIDsStrategyNotInIoCTest()
    {        
        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void NotBlockingCollectionTest()
    {
        var Any = new object();

        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);
    }

    [Fact]
    public void ErrorInThreadsIDsStrategyTest()
    {
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();
        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void SoftStopStrategyNotInIoCTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act); //Как тогда проверить?
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    [Fact]
    public void NotICommandSoftStopTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        var Any = new object();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    [Fact]
    public void ErrorInSoftStopTheThreadStrategyTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand> ("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    [Fact]
    public void CallExceptionTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() { 0 };
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockWaitForStopAllThreadCommand = new();
        Mock<IStrategy> mockWaitForStopAllThread = new();
        mockWaitForStopAllThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockWaitForStopAllThreadCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WaitForStopAllThread",
            (object[] args) => mockWaitForStopAllThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Throws<Exception>().Verifiable();

        mockExceptionHandlerCommand.Setup(
            cmd => cmd.execute()
        ).Verifiable();

        cmd.execute();

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandlerCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
        mockWaitForStopAllThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void NotCallExceptionTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockCommandsSendCommandCommand = new();
        Mock<IStrategy> mockCommandsSendCommand = new();
        mockCommandsSendCommand.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCommandsSendCommandCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SendCommand",
            (object[] args) => mockCommandsSendCommand.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockWaitForStopAllThreadCommand = new();
        Mock<IStrategy> mockWaitForStopAllThread = new();
        mockWaitForStopAllThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockWaitForStopAllThreadCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WaitForStopAllThread",
            (object[] args) => mockWaitForStopAllThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Verifiable();

        cmd.execute();

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandsSendCommand.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockWaitForStopAllThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ExceptionHandlerStrategyNotInIoCTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Throws<Exception>().Verifiable();

        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void NotICommandExceptionHandlerTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0, 1, 2};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        var Any = new object();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Throws<Exception>().Verifiable();
            
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ErrorInExceptionHandlerTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Throws<Exception>().Verifiable();

        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
        
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void ErrorInStopCommandTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() {0};
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Throws<Exception>().Verifiable();

        mockExceptionHandlerCommand.Setup(
            cmd => cmd.execute()
        ).Throws<InvalidOperationException>().Verifiable();

        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandlerCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }

    [Fact]
    public void WaitForStopAllThreadNotInIoCTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() { 0 };
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        var act = () => cmd.execute();

        Assert.Throws<ArgumentException>(act);

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void NotICommandWaitForStopAllThreadTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() { 0 };
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        var Any = new object();
        Mock<IStrategy> mockWaitForStopAllThread = new();
        mockWaitForStopAllThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WaitForStopAllThread",
            (object[] args) => mockWaitForStopAllThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        var act = () => cmd.execute();

        Assert.Throws<InvalidCastException>(act);

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockWaitForStopAllThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    [Fact]
    public void ErrorInWaitForStopAllThreadTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() { 0 };
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockWaitForStopAllThread = new();
        mockWaitForStopAllThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WaitForStopAllThread",
            (object[] args) => mockWaitForStopAllThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        var act = () => cmd.execute();

        Assert.Throws<InvalidOperationException>(act);

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockWaitForStopAllThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    [Fact]
    public void ErrorInWaitForStopAllThreadCommandTest()
    {
        BlockingCollection<int> idsArray = new BlockingCollection<int>() { 0 };
        Mock<IStrategy> mockThreadsIDs = new();
        mockThreadsIDs.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(idsArray).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadsIDs",
            (object[] args) => mockThreadsIDs.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockSoftStopCommand = new();
        Mock<IStrategy> mockSoftStopTheThread = new();
        mockSoftStopTheThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockSoftStopCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread",
            (object[] args) => mockSoftStopTheThread.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockExceptionHandlerCommand = new();
        Mock<IStrategy> mockExceptionHandler = new();
        mockExceptionHandler.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockExceptionHandlerCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler",
            (object[] args) => mockExceptionHandler.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockWaitForStopAllThreadCommand = new();
        Mock<IStrategy> mockWaitForStopAllThread = new();
        mockWaitForStopAllThread.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockWaitForStopAllThreadCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WaitForStopAllThread",
            (object[] args) => mockWaitForStopAllThread.Object.RunStrategy(args)
        ).Execute();

        StopServerCommand cmd = new StopServerCommand();

        mockWaitForStopAllThreadCommand.Setup(
            cmd => cmd.execute()
        ).Throws<InvalidOperationException>().Verifiable();

        var act = () => cmd.execute();

        Assert.Throws<InvalidOperationException>(act);

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockWaitForStopAllThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockWaitForStopAllThreadCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }
}
