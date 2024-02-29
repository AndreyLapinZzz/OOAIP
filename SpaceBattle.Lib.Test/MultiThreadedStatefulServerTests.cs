using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace SpaceBattle.Lib.Test;


public class MultiThreadedStategulServerTests
{
    Mock<IStrategy> exceptionHandlerStrategy = new Mock<IStrategy>();
    public MultiThreadedStategulServerTests()
    {

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.execute());

        var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateAndStartThread", (object[] args) => CreateAndStartThreadStrategy.RunStrategy(args)).Execute();
        var sendCommandStrategy = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SendCommand", (object[] args) => sendCommandStrategy.RunStrategy(args)).Execute();
        var hardStopThreadStrategy = new HardStopThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.HardStopThreadStrategy", (object[] args) => hardStopThreadStrategy.RunStrategy(args)).Execute();
        var softStopThreadStrategy = new HardStopThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SoftStopThreadStrategy", (object[] args) => softStopThreadStrategy.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandlerStrategy.Object.RunStrategy(args)).Execute();
    }


    public void LongRunningOperation(CancellationToken token)
    {
        while(!token.IsCancellationRequested) {
            Thread.Sleep(500);
        }
    }

    [Fact]
    public void ReceiveAdapterCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var cmd = new Mock<ICommand>();

        receiver.Object.Push(cmd.Object);

        var command = receiver.Object.Receive();

        Assert.True(receiver.Object.isEmpty());
        Assert.Equal(cmd.Object, command);
    }

    [Fact]
    public void UpdateBehaviourCheck()
    {
        Action doNothing = () => {};
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        var ubc = new Mock<UpdateBehaviourCommand>(thread.Object, doNothing);

        ubc.Object.execute();

        Assert.Equal(thread.Object.strategy, doNothing);
    }

    [Fact]
    public void HardStopCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        var cmd = new Mock<ICommand>();

        thread.Object.queue.Push(cmd.Object);

        var hardStopThreadStrategy = new HardStopThreadStrategy();
        ICommand hs = (ICommand)hardStopThreadStrategy.RunStrategy(thread.Object);
        hs.execute();

        Assert.True(thread.Object.stop);
        Assert.False(receiver.Object.isEmpty());
    }

    [Fact]
    public void SoftStopCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        Assert.Equal(thread.Object.queue, receiver.Object);

        CancellationTokenSource tokenSource = new();
        thread.Object.thread = new(
            () => LongRunningOperation(tokenSource.Token)
        );

        var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
        ICommand cast = IoC.Resolve<ICommand>("Game.CreateAndStartThread", thread.Object);
        cast.execute();

        var softStopThreadStrategy = new SoftStopThreadStrategy();
        ICommand ss = (ICommand)softStopThreadStrategy.RunStrategy(thread.Object);
        ss.execute();

        ICommand hs = receiver.Object.Receive();
        hs.execute();

        Assert.True(thread.Object.stop);
        
        Thread.Sleep(1500);
        tokenSource.Cancel();
        thread.Object.thread.Join();
        tokenSource.Dispose();
        Thread.Sleep(1500);

        Assert.True(receiver.Object.isEmpty());
    }

    [Fact]
    public void ExceptionCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        var cmd = new Mock<ICommand>();

        thread.Object.queue.Push(cmd.Object);
        thread.Object.strategy();

        cmd.Setup(x => x.execute()).Throws<Exception>().Verifiable();
        thread.Object.queue.Push(cmd.Object);
        thread.Object.strategy();
        exceptionHandlerStrategy.Verify();
    }

    [Fact]
    public void ServerStartCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);

        CancellationTokenSource tokenSource = new();
        thread.Object.thread = new(
            () => LongRunningOperation(tokenSource.Token)
        );
        var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
        ICommand cast = IoC.Resolve<ICommand>("Game.CreateAndStartThread", thread.Object);
        cast.execute();

        Thread.Sleep(1500);
        tokenSource.Cancel();
        thread.Object.thread.Join();
        tokenSource.Dispose();
        Thread.Sleep(1500);
    }
}
