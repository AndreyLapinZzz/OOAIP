using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System.Windows.Input;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace SpaceBattle.Lib.Test;

public class StopServerCommandTests
{
    public StopServerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }
    //1)Нет такой стратегии в айоке
    [Fact]
    public void ThreadsIDsStrategyNotInIoCTest()
    {        
        StopServerCommand cmd = new StopServerCommand();
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
    }
    //2)Возвращает не тот тип
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

    //3)Ошибка внутри стратегии
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

    //1)Нет такой стратегии в айоке
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
    //2)Возвращает не тот тип
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
    //3)Ошибка внутри стратегии
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
    //Если вызывает ексепшен
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
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandlerCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }
    //2) Не вызывает
    [Fact]
    public void NotCallExceptionTest()
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

        StopServerCommand cmd = new StopServerCommand();

        mockSoftStopCommand.Setup(
            cmd => cmd.execute()
        ).Verifiable();

        cmd.execute();

        mockThreadsIDs.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockSoftStopTheThread.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(3));
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(3));
    }
    //1)Нет такой стратегии в айоке
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
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }

    //2)Возвращает не тот тип
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
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }
    //3)Ошибка внутри стратегии
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
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    //4) Внутри айкоманда ошибка
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
        mockSoftStopCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
        mockExceptionHandler.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockExceptionHandlerCommand.Verify(cmd => cmd.execute(), Times.Exactly(1));
    }

    // Нет позитивного теста
}
