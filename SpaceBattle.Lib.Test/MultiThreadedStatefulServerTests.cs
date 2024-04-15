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
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    [Fact]
    public void HardStopCheck()
    {
        bool actioncall = false;
        var queue = new BlockingCollection<ICommand>();
        var sender = new SendCommand(queue);
        var reciever = new ReceiveAdapter(queue);
        var thread = new MyThread(reciever);
        var thread_tree_Strategy = new Mock<IStrategy>();
        thread_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(thread);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThread", (object[] args) => thread_tree_Strategy.Object.RunStrategy(args)).Execute();
        var sender_tree_Strategy = new Mock<IStrategy>();
        sender_tree_Strategy.Setup(m => m.RunStrategy("Thread_id")).Returns(sender);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.GetSender", (object[] args) => sender_tree_Strategy.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Thread.SendCommand", (object[] args) => new SendCommandStrategy().RunStrategy(args)).Execute();
        var hs_strategy = new HardStopThreadStrategy();
        Assert.True(reciever.isEmpty());
        var hs = (ActionCommand)hs_strategy.RunStrategy("Thread_id", ()=>{actioncall=true;});
        hs.execute();
        Assert.False(reciever.isEmpty());
        Assert.True(actioncall);
    }

    [Fact]
    public void ExceptionCheck()
    {
        var queue = new BlockingCollection<ICommand>();
        var sender = new SendCommand(queue);
        var reciever = new ReceiveAdapter(queue);
        var thread = new MyThread(reciever);
        var cmd = new Mock<ICommand>();

        sender.Send(cmd.Object);
        cmd.Setup(x => x.execute()).Throws<Exception>().Verifiable();
        sender.Send(cmd.Object);
        thread.strategy();
        exceptionHandler.Verify();
    }

    [Fact]
    public void SoftStopCheck()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var reciever = new ReceiveAdapter(queue);
        var sender = new SendCommand(queue);
        var thread = new MyThread(reciever);
        var sf = new SoftStopCommand(thread);
        var cv = new ManualResetEvent(false);
        var cv1 = new ManualResetEvent(false);
        var cmd = new ActionCommand((arg) =>
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            cv.WaitOne();
            sf.execute();
        });
        bool wasCalled = false;
        var mcmd = new Mock<ICommand>();
        mcmd.Setup(m => m.execute()).Callback(() => { wasCalled = true; cv1.Set(); });
        thread.execute();
        sender.Send(cmd);
        sender.Send(mcmd.Object);
        cv.Set();
        Assert.True(cv1.WaitOne(10000));
        Assert.True(wasCalled);
    }

    [Fact]
    public void ThreadSoftStopStrategyCheck()
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
        var sfts_strategy = new SoftStopThreadStrategy();
        Assert.True(reciever.isEmpty());
        var sfts = (ActionCommand)sfts_strategy.RunStrategy("Thread_id", ()=>{actioncall=true;});
        sfts.execute();
        Assert.False(reciever.isEmpty());
        Assert.True(actioncall);
    }

    [Fact]
    public void ThreadCreateAndStartTest()
    {
        var queue = new BlockingCollection<ICommand>();
        var reciever = new ReceiveAdapter(queue);
        var send = new SendCommand(queue);
        var thread = new MyThread(reciever);
        var hs = new HardStopCommand(thread);
        var cv = new ManualResetEvent(false);
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

    [Fact]
    public void ThreadCreateAndStartStrategyWithActionTest()
    {
        bool actioncall = false;
        var thread_dict = new Dictionary<string, MyThread>();
        var sender_dict = new Dictionary<string, ISender>();
        var get_thread_list_strategy = new Mock<IStrategy>();
        var get_sender_list_strategy = new Mock<IStrategy>();
        get_sender_list_strategy.Setup(m => m.RunStrategy()).Returns(sender_dict);
        get_thread_list_strategy.Setup(m => m.RunStrategy()).Returns(thread_dict);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetThreadList", (object[] args) => get_thread_list_strategy.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetSenderList", (object[] args) => get_sender_list_strategy.Object.RunStrategy(args)).Execute();
        var cast_strategy = new CreateAndStartThreadStrategy();
        var cast = (ActionCommand)cast_strategy.RunStrategy("Thread_id",  ()=>{actioncall=true;});
        cast.execute();
        Assert.True(thread_dict.ContainsKey("Thread_id"));
        var thread = thread_dict["Thread_id"];
        Assert.Equal(typeof(MyThread), thread.GetType());
        Assert.True(actioncall);

        var send = sender_dict["Thread_id"];
        var hs = new HardStopCommand(thread);
        send.Send(hs);

    }
}
