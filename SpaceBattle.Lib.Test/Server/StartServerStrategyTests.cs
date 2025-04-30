using Hwdtech;
using Hwdtech.Ioc;
using System;

namespace SpaceBattle.Lib.Test;

public class StartServerStrategyTests
{
    public StartServerStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void NotUIntTest()
    {
        object[] args = new object[] { "1" };
        StartServerStrategy sss = new StartServerStrategy();
        Assert.Throws<InvalidCastException>(() => sss.RunStrategy(args));
    }

    [Fact]
    public void NoArgsTest()
    {
        StartServerStrategy sss = new StartServerStrategy();
        Assert.Throws<IndexOutOfRangeException>(() => sss.RunStrategy());
    }

    [Fact]
    public void OkStartServerStrategyTest()
    {
        object[] args = { (uint)1, "chto-to"};
        StartServerStrategy sss = new StartServerStrategy();
        var result = sss.RunStrategy(args);
        Assert.IsType<StartServerCommand>(result);
    }
}
