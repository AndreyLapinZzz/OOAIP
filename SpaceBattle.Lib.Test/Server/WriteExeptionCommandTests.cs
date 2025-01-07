using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System.Windows.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace SpaceBattle.Lib.Test;

public class WriteExeptionCommandTests
{
    public WriteExeptionCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    [Fact]
    public void GetInfoExeptionStrategyNotInIoCTest()
    {
        Mock<TextWriter> mockTextWriter = new();
        mockTextWriter.Setup(
            writer => writer.WriteLine(It.IsAny<object>())
        ).Verifiable();

        Mock<IStrategy> mockGetLogStream = new();
        mockGetLogStream.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockTextWriter.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetLogStream",
            (object[] args) => mockGetLogStream.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
        mockTextWriter.Verify(writer => writer.WriteLine(It.IsAny<object>()), Times.Never());
        mockGetLogStream.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Never());
    }

    [Fact]
    public void NotIEnumerableTest()
    {
        var Any = new object();

        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);
    }

    [Fact]
    public void ErrorInGetInfoExeptionStrategyTest()
    {
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();
        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void GetLogStreamStrategyNotInIoCTest()
    {
        List<object> data = new List<object> { "Str1", "Str2", "Str3" };
        IEnumerable<object> info = data;
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(info).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();
        Mock<TextWriter> mockTextWriter = new();
        mockTextWriter.Setup(
            writer => writer.WriteLine(It.IsAny<object>())
        ).Verifiable();
        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<ArgumentException>(act);
        mockGetInfoExeption.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockTextWriter.Verify(writer => writer.WriteLine(It.IsAny<object>()), Times.Never());
    }

    [Fact]
    public void NotTextWriterTest()
    {
        List<object> data = new List<object> { "Str1", "Str2", "Str3" };
        IEnumerable<object> info = data;
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(info).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();

        var Any = new object();
        Mock<IStrategy> mockGetLogStream = new();
        mockGetLogStream.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetLogStream",
            (object[] args) => mockGetLogStream.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<InvalidCastException>(act);
    }

    [Fact]
    public void ErrorInGetLogStreamStrategyTest()
    {
        List<object> data = new List<object> { "Str1", "Str2", "Str3" };
        IEnumerable<object> info = data;
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(info).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockGetLogStream = new();
        mockGetLogStream.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetLogStream",
            (object[] args) => mockGetLogStream.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void ErrorInWriteLineTest()
    {
        List<object> data = new List<object> { "Str1", "Str2", "Str3" };
        IEnumerable<object> info = data;
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(info).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();

        Mock<TextWriter> mockTextWriter = new();
        mockTextWriter.Setup(
            writer => writer.WriteLine(It.IsAny<object>())
        ).Throws<InvalidOperationException>().Verifiable();

        Mock<IStrategy> mockGetLogStream = new();
        mockGetLogStream.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockTextWriter.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetLogStream",
            (object[] args) => mockGetLogStream.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        var act = () => cmd.execute();
        Assert.Throws<InvalidOperationException>(act);
        mockGetInfoExeption.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockGetLogStream.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void OkWriteExceptionComandTest()
    {
        List<object> data = new List<object> { "Str1", "Str2", "Str3" };
        IEnumerable<object> info = data;
        Mock<IStrategy> mockGetInfoExeption = new();
        mockGetInfoExeption.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(info).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInfoExeption",
            (object[] args) => mockGetInfoExeption.Object.RunStrategy(args)
        ).Execute();

        Mock<TextWriter> mockTextWriter = new();
        mockTextWriter.Setup(
            writer => writer.WriteLine(It.IsAny<object>())
        ).Verifiable();

        Mock<IStrategy> mockGetLogStream = new();
        mockGetLogStream.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockTextWriter.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetLogStream",
            (object[] args) => mockGetLogStream.Object.RunStrategy(args)
        ).Execute();

        Exception ex = new Exception();
        WriteExeptionCommand cmd = new WriteExeptionCommand(ex);
        cmd.execute();
        mockGetInfoExeption.Verify(strategy => strategy.RunStrategy(It.Is<object[]>(p => p[0] == ex)), Times.Exactly(1));
        mockTextWriter.Verify(writer => writer.WriteLine(It.Is<object>(param => data.Contains(param))), Times.Exactly(3));
        mockGetLogStream.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
}
