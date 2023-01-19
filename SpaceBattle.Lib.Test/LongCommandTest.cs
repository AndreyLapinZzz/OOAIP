using Moq;
using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;

public class LongCommandTest
{
    public LongCommandTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }
    [Fact]
    public void LongCommandStrategyTest(){
        var mockComf = new Mock<ICommand>();
        
        RepeatCommand repCom = new RepeatCommand(mockComf.Object);
        repCom.execute();

        var mockQueuePushStrategy = new Mock<IStrategy>();
        mockQueuePushStrategy.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(repCom).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.QueuePushBack", (object[] args) => mockQueuePushStrategy.Object.RunStrategy(args)).Execute();

        var mockMComStrategy = new Mock<IStrategy>();
        mockMComStrategy.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(repCom).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateMacroCommand", (object[] args) => mockMComStrategy.Object.RunStrategy(args)).Execute();
    
        var mockRepStrategy = new Mock<IStrategy>();
        mockRepStrategy.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(repCom).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CommandRepeat", (object[] args) => mockRepStrategy.Object.RunStrategy(args)).Execute();
        
        var LongCommand = new LongCommandStrategy();
        var mockUObj = new Mock<IUObject>();
        LongCommand.RunStrategy(It.IsAny<string>(), mockUObj.Object);
        mockQueuePushStrategy.Verify();
        mockMComStrategy.Verify();
        mockRepStrategy.Verify();
    }
}
