using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System.Windows.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Xunit;
using SpaceBattle.Lib;

namespace Endpoint.Test;

public class EndPointTests
{
    public EndPointTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    //1)Нет такой стратегии в айоке
    [Fact]
    public void GameGetThreadIDByGameIDNotInIoCTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;
        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);
        Assert.Throws<ArgumentException>(act);
    }

    //2)Ошибка внутри стратегии
    [Fact]
    public void ErrorInGameGetThreadIDByGameIDTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);

        Assert.Throws<InvalidOperationException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //3)Нет такой стратегии в айоке
    [Fact]
    public void CommandCreateFromMessageNotInIoCTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);
        Assert.Throws<ArgumentException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //4)Ошибка внутри стратегии
    [Fact]
    public void ErrorInCommandCreateFromMessageTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);

        Assert.Throws<InvalidOperationException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //5)Нет такой стратегии в айоке
    [Fact]
    public void ThreadSendCmdNotInIoCTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);
        Assert.Throws<ArgumentException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //6)Возвращает не тот тип 
    [Fact]
    public void NotICommandThreadSendCmdTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        var Any = new object();

        Mock<IStrategy> mockThreadSendCmd = new();
        mockThreadSendCmd.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCmd",
            (object[] args) => mockThreadSendCmd.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);
        Assert.Throws<InvalidCastException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmd.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //7)Ошибка внутри стратегии 
    [Fact]
    public void ErrorInThreadSendCmdTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockThreadSendCmd = new();
        mockThreadSendCmd.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCmd",
            (object[] args) => mockThreadSendCmd.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);

        Assert.Throws<InvalidOperationException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmd.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //8)Некорректные параметры
    [Fact]
    public void IncorrectParamTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockThreadSendCmd = new();
        mockThreadSendCmd.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<ArgumentException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCmd",
            (object[] args) => mockThreadSendCmd.Object.RunStrategy(args)
        ).Execute();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);

        Assert.Throws<ArgumentException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmd.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //9)ошибка внутри айкомманда
    [Fact]
    public void ErrorInThreadSendCmdCommandTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockThreadSendCmdCommand = new();
        Mock<IStrategy> mockThreadSendCmd = new();
        mockThreadSendCmd.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockThreadSendCmdCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCmd",
            (object[] args) => mockThreadSendCmd.Object.RunStrategy(args)
        ).Execute();

        mockThreadSendCmdCommand.Setup(
            cmd => cmd.Execute()
        ).Throws<InvalidOperationException>().Verifiable();

        EndPoint cmd = new EndPoint();
        var act = () => cmd.HttpCommand(message);

        Assert.Throws<InvalidOperationException>(act);
        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmdCommand.Verify(cmd => cmd.Execute(), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmd.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }


    //OK
    [Fact]
    public void OKEndPointTest()
    {
        Mock<Message> mockParam = new();
        Message message = mockParam.Object;

        Mock<IStrategy> mockGameGetThreadIDByGameID = new();
        mockGameGetThreadIDByGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns("threadIDs").Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetThreadIDByGameID",
            (object[] args) => mockGameGetThreadIDByGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockCMD = new();
        Mock<IStrategy> mockCommandCreateFromMessage = new();
        mockCommandCreateFromMessage.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCMD.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CreateFromMessage",
            (object[] args) => mockCommandCreateFromMessage.Object.RunStrategy(args)
        ).Execute();

        Mock<Hwdtech.ICommand> mockThreadSendCmdCommand = new();
        Mock<IStrategy> mockThreadSendCmd = new();
        mockThreadSendCmd.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockThreadSendCmdCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCmd",
            (object[] args) => mockThreadSendCmd.Object.RunStrategy(args)
        ).Execute();

        mockThreadSendCmdCommand.Setup(
            cmd => cmd.Execute()
        ).Verifiable();

        EndPoint cmd = new EndPoint();
        cmd.HttpCommand(message);

        mockGameGetThreadIDByGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmdCommand.Verify(cmd => cmd.Execute(), Times.Exactly(1));
        mockCommandCreateFromMessage.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockThreadSendCmd.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
}
