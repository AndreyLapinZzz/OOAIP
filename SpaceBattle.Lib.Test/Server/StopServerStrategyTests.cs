using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System;
using System.Windows.Input;

namespace SpaceBattle.Lib.Test;

public class StopServerStategyTests
{
    public StopServerStategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    [Fact]
    public void OkStopServerStrategyTest()
    {
        StopServerStrategy sss = new StopServerStrategy();
        var result = sss.RunStrategy();
        Assert.IsType<StopServerCommand>(result);
    }
}