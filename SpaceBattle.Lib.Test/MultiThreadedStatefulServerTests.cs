using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace SpaceBattle.Lib.Test;


public class MultiThreadedStategulServerTests
{
    Mock<IStrategy> exceptionHandler = new Mock<IStrategy>();
    public MultiThreadedStategulServerTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        // var m_exceptionHandler = new Mock<ExceptionHandler>();
        // var m_handler = new Mock<IStrategy>();
        // m_handler.Setup(m => m.уxecute(It.IsAny<object[]>())).Returns(m_exceptionHandler.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    [Fact]
    public void HardStopCheck()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var sender = new SendCommand(queue);
        var reciever = new ReceiveAdapter(queue);
        var thread = new MyThread(reciever);
        var m_thread_tree_Strategy = new Mock<IStrategy>();
        m_thread_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(thread);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThread", (object[] args) => m_thread_tree_Strategy.Object.RunStrategy(args)).Execute();
        var m_sender_tree_Strategy = new Mock<IStrategy>();
        m_sender_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => m_sender_tree_Strategy.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) => new SendCommandStrategy().RunStrategy(args)).Execute();
        var hs_strategy = new HardStopThreadStrategy();
        Assert.True(reciever.isEmpty());
        var hs = (ActionCommand)hs_strategy.RunStrategy("Thread_id");
        hs.execute();
        Assert.False(reciever.isEmpty());
    }

    [Fact]
    public void ThreadHardStopStrategyWithActionTest()
    {
        bool actioncall = false;
        var queue = new BlockingCollection<ICommand>(100);
        var sender = new SendCommand(queue);
        var reciever = new ReceiveAdapter(queue);
        var thread = new MyThread(reciever);
        var m_thread_tree_Strategy = new Mock<IStrategy>();
        m_thread_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(thread);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThread", (object[] args) => m_thread_tree_Strategy.Object.RunStrategy(args)).Execute();
        var m_sender_tree_Strategy = new Mock<IStrategy>();
        m_sender_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => m_sender_tree_Strategy.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) => new SendCommandStrategy().RunStrategy(args)).Execute();
        var hs_strategy = new HardStopThreadStrategy();
        Assert.True(reciever.isEmpty());
        var hs = (ActionCommand)hs_strategy.RunStrategy("Thread_id", ()=>{actioncall=true;});
        hs.execute();
        Assert.False(reciever.isEmpty());
        Assert.True(actioncall);
    }

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
