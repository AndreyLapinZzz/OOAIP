using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace SpaceBattle.Lib.Test;


public class MultiThreadedStategulServerTests
{
    //public Mock<IStrategy> HardStopThreadStrategy = new Mock<IStrategy>();
    //Mock<IStrategy> HardStopThreadStrategy = new Mock<IStrategy>();
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
    }


    public void LongRunningOperation(CancellationToken token)
    {
        while(!token.IsCancellationRequested) { // Check if the caller requested cancellation. 
            //Console.WriteLine("I'm running");
            Thread.Sleep(500);
        }
    }

    [Fact]
    public void ReceiveAdapterCheck()
    {
        //ManualResetEvent mre = new ManualResetEvent(false);

        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var cmd = new Mock<ICommand>();

        receiver.Object.Push(cmd.Object);

        var command = receiver.Object.Receive();

        Assert.True(receiver.Object.isEmpty());
        Assert.Equal(cmd.Object, command);

        //var thread = new Mock<MyThread>(receiver.Object);

        //thread.Object.execute();
        
        //receiver.Object.Push(cmd.Object);

        //var a = Thread.CurrentThread.IsAlive;

        //Assert.Throws<Exception>(() => receiver.Object.Receive());
        

    }

    // [Fact]
    // public void SendCommandCheck()
    // {
    //     var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
    //     var cmd = new Mock<ICommand>();
    //     var thread = new Mock<MyThread>(receiver.Object);

    //     // var SendCommand = new SendCommand(thread.Object, cmd.Object);
    //     // SendCommand.execute();

    //     // var command = receiver.Object.Receive();

    //     // Assert.Equal(cmd.Object, command);
    //     var sendCommandStrategy = new SendCommandStrategy();
    //     ICommand sendCommand = (ICommand)sendCommandStrategy.RunStrategy(thread.Object, cmd.Object);
    //     sendCommand.execute();

    //     //IoC.Resolve<ICommand>("Game.SendCommand", thread.Object, cmd.Object);
    //     var command = receiver.Object.Receive();
    //     Assert.Equal(cmd.Object, command);
    // }

    [Fact]
    public void UpdateBehaviourCheck()
    {
        Action donothing = () => {};
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        var ubc = new Mock<UpdateBehaviourCommand>(thread.Object, donothing);

        ubc.Object.execute();

        Assert.Equal(thread.Object.strategy, donothing);
    }

    // [Fact]
    // public void HardStopCheck()
    // {
    //     var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
    //     var thread = new Mock<MyThread>(receiver.Object);
    //     Assert.Equal(thread.Object.queue, receiver.Object);

    //     //Assert.Throws<Exception>(() => thread.Object.execute());
    //     //thread.Object.execute();
    //     //Assert.True(thread.Object.thread.IsAlive);

    //     var hardStopThreadStrategy = new HardStopThreadStrategy();
    //     ICommand hs = (ICommand)hardStopThreadStrategy.RunStrategy(thread.Object);
    //     hs.execute();

    //     Assert.True(thread.Object.stop);



    //     //var hs = new Mock<HardStopCommand>(thread.Object);
    //     //receiver.Object.Push(hs.Object);
    //     //Assert.False(thread.Object.thread.IsAlive);

    //     // var SendCommand = new SendCommand(thread.Object, hs.Object);
    //     // var HardStopThreadStrategy = new HardStopThreadStrategy();
    //     // SendCommand.execute();

    //     //HardStopCommandStrategy.Setup(c => c.RunStrategy()).Returns(0).Verifiable();
    // }

    [Fact]
    public void SoftStopCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        Assert.Equal(thread.Object.queue, receiver.Object);

        var softStopThreadStrategy = new SoftStopThreadStrategy();
        ICommand ss = (ICommand)softStopThreadStrategy.RunStrategy(thread.Object);
        ss.execute();

        ICommand hs = receiver.Object.Receive();
        hs.execute();

        Assert.True(thread.Object.stop);
    }

    [Fact]
    public void ExceptionCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);
        var cmd = new Mock<ICommand>();

        cmd.Setup(x => x.execute()).Throws<Exception>().Verifiable();
        thread.Object.queue.Push(cmd.Object);
        Assert.Throws<Exception>(() => thread.Object.strategy());
    }

    [Fact]
    public void ServerStartCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var thread = new Mock<MyThread>(receiver.Object);

        CancellationTokenSource tokenSource = new();
        thread.Object.thread = new(
            () => LongRunningOperation(tokenSource.Token) // Pass the token to the thread you want to stop.
        );
        var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
        ICommand cast = IoC.Resolve<ICommand>("Game.CreateAndStartThread", thread.Object);
        cast.execute();

        Thread.Sleep(1500);
        tokenSource.Cancel(); // Request cancellation. 
        thread.Object.thread.Join(); // If you want to wait for cancellation, `Join` blocks the calling thread until the thread represented by this instance terminates.
        tokenSource.Dispose(); // Dispose the token source.
        Thread.Sleep(1500);
    }
}