using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class EndMoveCommandTest{
    public EndMoveCommandTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.execute());

        var mockStrategyEmpty = new Mock<IStrategy>();
        mockStrategyEmpty.Setup(x => x.RunStrategy()).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Empty", (object[] args) => mockStrategyEmpty.Object.RunStrategy(args)).Execute();

        var mockStrategyInject = new Mock<IStrategy>();
        mockStrategyInject.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockStrategyInject.Object.RunStrategy(args)).Execute();

        var mockStrategyDelete = new Mock<IStrategy>();
        mockStrategyDelete.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(mockCommand.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.DeleteProperty", (object[] args) => mockStrategyDelete.Object.RunStrategy(args)).Execute();
    }
    [Fact]
    public void EndMoveCommandT1(){
        var mockEnd = new Mock<IMoveCommandEndable>();
        var mockObj = new Mock<IUObject>();
        var mockCom = new Mock<ICommand>();
        mockEnd.SetupGet(x => x.obj).Returns(mockObj.Object);
        mockEnd.SetupGet(x => x.com).Returns(mockCom.Object);
        ICommand EMC = new EndMoveCommand(mockEnd.Object);
        EMC.execute();
        mockEnd.VerifyAll();
    }

    [Fact]
    public void EndMoveCommandT2(){
        var mockEnd = new Mock<IMoveCommandEndable>();
        var mockObj = new Mock<IUObject>();
        var mockCom = new Mock<ICommand>();
        mockEnd.SetupGet(x => x.obj).Throws<Exception>();
        mockEnd.SetupGet(x => x.obj).Returns(mockObj.Object);
        mockEnd.SetupGet(x => x.com).Throws<Exception>();
        ICommand EMCExcep = new EndMoveCommand(mockEnd.Object);
        Assert.Throws<Exception>(() => EMCExcep.execute());
        ICommand EndMoveCommandComException = new EndMoveCommand(mockEnd.Object);
        Assert.Throws<Exception>(() => EndMoveCommandComException.execute());
    }
}
