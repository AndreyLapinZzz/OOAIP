using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;


public class MultiThreadedStategulServerTests
{
    //public Mock<IStrategy> HardStopThreadStrategy = new Mock<IStrategy>();
    Mock<IStrategy> HardStopThreadStrategy = new Mock<IStrategy>();
    public MultiThreadedStategulServerTests()
    {

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.execute());

        var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateAndStartThread", (object[] args) => CreateAndStartThreadStrategy.RunStrategy(args)).Execute();
        var SendCommandStrategy = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SendCommand", (object[] args) => SendCommandStrategy.RunStrategy(args)).Execute();
        //var HardStopThreadStrategy = new HardStopThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.HardStopThreadStrategy", (object[] args) => HardStopThreadStrategy.Object.RunStrategy(args)).Execute();
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

    [Fact]
    public void SendCommandCheck()
    {
        var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
        var cmd = new Mock<ICommand>();
        var thread = new Mock<MyThread>(receiver.Object);

        var SendCommand = new SendCommand(thread.Object, cmd.Object);
        SendCommand.execute();

        var command = receiver.Object.Receive();

        Assert.Equal(cmd.Object, command);

    }

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
    //     //HardStopThreadStrategy.Setup(x => x.RunStrategy(thread)).Returns(0).Verifiable();        

    //     //Assert.Throws<Exception>(() => thread.Object.execute());
    //     thread.Object.execute();
    //     Assert.True(thread.Object.thread.IsAlive);

    //     var hs = new Mock<HardStopCommand>(thread.Object);
    //     //Assert.Equal(0, hs.Object.execute());

    //     receiver.Object.Push(hs.Object);
    //     // var SendCommand = new SendCommand(thread.Object, hs.Object);
    //     // var HardStopThreadStrategy = new HardStopThreadStrategy();
    //     // SendCommand.execute();

    //     //var HardStopThreadStrategy = new HardStopThreadStrategy();

    //     Assert.False(thread.Object.thread.IsAlive);

    //     //Thread.Sleep(1000);

    //     //HardStopCommandStrategy.Setup(c => c.RunStrategy()).Returns(0).Verifiable();
    // }


//     [Fact]
//     public void ServerStartCheck()
//     {
//         var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
//         var thread = new Mock<MyThread>(receiver.Object);
//         thread.Object.execute();
//         Assert.True(thread.Object.thread.IsAlive);

//         //var cmd = new Mock<ICommand>();
//         //cmd.Setup(c => c.Execute()).Callback(cv => cv.Notify).Verifiable();
//     }
}