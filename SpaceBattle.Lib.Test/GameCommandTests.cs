using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;
public class GameCommandTests
{   
    public GameCommandTests(){
       
    }
    [Fact]
    public void GameCommandTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope1 = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        var game_scope = IoC.Resolve<object>("Scopes.New", scope1);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope).Execute();
        var m_handler_strat = new Mock<IStrategy>();
        var handler_called = false;
        m_handler_strat.Setup(m => m.RunStrategy(It.IsAny<object[]>())).Callback(()=>{handler_called = true;});
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => m_handler_strat.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(400).RunStrategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope1).Execute();
        
        var cmd = new ActionCommand((arg) =>
        {   
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(0).RunStrategy()).Execute();
        });
        var queue = new BlockingCollection<ICommand>(100)
        {
            cmd,
            cmd
        };
        var receiver = new ReceiveAdapter(queue);
        var game  = new GameCommand("GameId", game_scope, receiver);
        game.execute();
        Assert.False(receiver.isEmpty());
        Assert.False(handler_called);
    }
    [Fact]
    public void GameCommandHandleTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope1 = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        var game_scope1 = IoC.Resolve<object>("Scopes.New", scope1);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", game_scope1).Execute();
        var m_handler_strat = new Mock<IStrategy>();
        bool handler_called = false;
        m_handler_strat.Setup(m=>m.RunStrategy(It.IsAny<object[]>())).Callback(()=>{handler_called = true;});
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => m_handler_strat.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(400).RunStrategy()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope1).Execute();
        
        var cmd = new Mock<ICommand>();
        var cmd1 = new ActionCommand((arg) =>
        {   
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameTimeLimit", (object[] args) => new GetGameTimeLimitStrategy(0).RunStrategy()).Execute();
        });
        cmd.Setup(m=>m.execute()).Throws<Exception>();
        var queue = new BlockingCollection<ICommand>(100)
        {
            cmd.Object,
            cmd1
        };
        var receiver = new ReceiveAdapter(queue);
        var game  = new GameCommand("GameId", game_scope1, receiver);
        game.execute();
        Assert.True(handler_called);
    }
    
}
