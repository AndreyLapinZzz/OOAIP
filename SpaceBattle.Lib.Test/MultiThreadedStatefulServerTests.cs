using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace SpaceBattle.Lib.Test;


public class MultiThreadedStategulServerTests
{
    public MultiThreadedStategulServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    // [Fact]
    // public void HardStopCheck()
    // {
    //     var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
    //     var thread = new Mock<MyThread>(receiver.Object);
    //     var cmd = new Mock<ICommand>();
    //     var action = new Mock<Action>();

    //     thread.Object.queue.Push(cmd.Object);

    //     var hardStopThreadStrategy = new HardStopThreadStrategy();
    //     ICommand hs = (ICommand)hardStopThreadStrategy.RunStrategy(thread.Object, action.Object);

    //     Assert.True(thread.Object.stop);
    //     Assert.False(receiver.Object.isEmpty());
    // }

    // [Fact]
    // public void SoftStopCheck()
    // {
    //     var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
    //     var thread = new Mock<MyThread>(receiver.Object);
    //     var cmd1 = new Mock<ICommand>();
    //     var cmd2 = new Mock<ICommand>();
    //     Assert.Equal(thread.Object.queue, receiver.Object);

    //     var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
    //     ICommand cast = IoC.Resolve<ICommand>("Thread.CreateAndStartThread", thread.Object);
    //     cast.execute();

    //     var softStopThreadStrategy = new SoftStopThreadStrategy();
    //     ICommand ss = (ICommand)softStopThreadStrategy.RunStrategy(thread.Object);

    //     thread.Object.queue.Push(cmd1.Object);
    //     thread.Object.queue.Push(ss);
    //     thread.Object.queue.Push(cmd2.Object);

    //     thread.Object.queue.Receive().execute();
    //     thread.Object.queue.Receive().execute();

    //     Assert.False(receiver.Object.isEmpty());
    //     Assert.Equal(thread.Object.queue.Receive(), cmd2.Object);
    //     Assert.True(thread.Object.queue.isEmpty());

    //     Assert.True(thread.Object.stop);
    // }

    [Fact]
    public void ServerStartCheck()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new ReceiveAdapter(queue);
        var send = new SendCommand(queue);
        var thread = new MyThread(reciever);
        var cv = new ManualResetEvent(false);
        var hs = new HardStopCommand(thread);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            cv.Set();
            hs.execute();
        });
        send.Send(cmd);
        Assert.Single(queue);
        Assert.False(reciever.isEmpty());
        thread.execute();
        Assert.True(cv.WaitOne(10000));
        Assert.NotEqual(1, queue.Count);
        Assert.True(reciever.isEmpty());
    }

    // [Fact]
    // public void SendCommandCheck()
    // {
    //     var receiver = new Mock<ReceiveAdapter>(new Mock<BlockingCollection<ICommand>>().Object);
    //     var thread = new Mock<MyThread>(receiver.Object);
    //     var cmd = new Mock<ICommand>();

    //     var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
    //     ICommand cast = IoC.Resolve<ICommand>("Thread.CreateAndStartThread", thread.Object);
    //     cast.execute();

    //     var SendCommandStrategy = new SendCommandStrategy();
    //     ICommand send = IoC.Resolve<ICommand>("Thread.SendCommand", thread.Object, cmd.Object);
    //     thread.Object.queue.Push(send);
    //     Assert.False(thread.Object.queue.isEmpty());
    //     thread.Object.strategy();
    // }
}
