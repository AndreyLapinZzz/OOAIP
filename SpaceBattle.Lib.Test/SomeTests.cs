using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class SomeTest
{
    public SomeTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var someStrategy = new SomeStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.SomeStrategy", (object[] args) => someStrategy.RunStrategy(args)).Execute();
    }


    [Fact]
    public void newTest(){
        var someStrategy = new SomeStrategy();
        Assert.Equal(5, someStrategy.RunStrategy(2, 3));
    }
}
