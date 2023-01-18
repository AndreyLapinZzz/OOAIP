using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class EndMoveCommandTest{
    public EndMoveCommandTest(){
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Mock<IMovable> mockMovable = new();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        MoveCommand movCom = new MoveCommand(mockMovable.Object);
        movCom.execute();

        var mockStrategyEmpty = new Mock<IStrategy>();
        mockStrategyEmpty.Setup(x => x.RunStrategy()).Returns(movCom);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Empty", (object[] args) => mockStrategyEmpty.Object.RunStrategy(args)).Execute();

        var mockStrategyInject = new Mock<IStrategy>();
        mockStrategyInject.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(movCom);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => mockStrategyInject.Object.RunStrategy(args)).Execute();

        var mockStrategyDelete = new Mock<IStrategy>();
        mockStrategyDelete.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(movCom);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.DeleteProperty", (object[] args) => mockStrategyDelete.Object.RunStrategy(args)).Execute();
    }
    [Fact]
    public void EndMoveCommandT1(){
        var mockEnd = new Mock<IMoveCommandEndable>();
        var mockObj = new Mock<IUObject>();

        
        Mock<IMovable> mockMovable = new();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        MoveCommand movCom = new MoveCommand(mockMovable.Object);
        movCom.execute();

        mockEnd.SetupGet(x => x.obj).Returns(mockObj.Object);
        mockEnd.SetupGet(x => x.com).Returns(movCom);
        ICommand EMC = new EndMoveCommand(mockEnd.Object);
        EMC.execute();
        mockEnd.VerifyAll();
    }

    [Fact]
    public void EndMoveCommandT2(){
        var mockEnd = new Mock<IMoveCommandEndable>();
        var mockObj = new Mock<IUObject>();
        
        Mock<IMovable> mockMovable = new();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        MoveCommand movCom = new MoveCommand(mockMovable.Object);
        movCom.execute();

        mockEnd.SetupGet(x => x.obj).Throws<Exception>();
        mockEnd.SetupGet(x => x.obj).Returns(mockObj.Object);
        mockEnd.SetupGet(x => x.com).Throws<Exception>();
        ICommand EMCExcep = new EndMoveCommand(mockEnd.Object);
        Assert.Throws<Exception>(() => EMCExcep.execute());
        ICommand EndMoveCommandComException = new EndMoveCommand(mockEnd.Object);
        Assert.Throws<Exception>(() => EndMoveCommandComException.execute());
    }
}
