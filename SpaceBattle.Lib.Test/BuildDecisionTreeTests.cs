using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class DecisionTreeTests
{

    public DecisionTreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PositiveBuildDecisionTreeTest()
    {
        string path = "../../../Tree.txt";
        var getDecisionTreeStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetDecisionTree", (object[] args) => getDecisionTreeStrategy.Object.RunStrategy(args)).Execute();
        getDecisionTreeStrategy.Setup(t => t.RunStrategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdt = new BuildDecisionTree(path);

        bdt.execute();

        getDecisionTreeStrategy.Verify();
    }

    [Fact]
    public void NegativeBuildDecisionTreeTestThrowsException()
    {
        string path = "";
        var getDecisionTreeStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetDecisionTree", (object[] args) => getDecisionTreeStrategy.Object.RunStrategy(args)).Execute();
        getDecisionTreeStrategy.Setup(t => t.RunStrategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdt = new BuildDecisionTree(path);

        Assert.Throws<Exception>(() => bdt.execute());

        getDecisionTreeStrategy.Verify();
    }

    [Fact]
    public void NegativeBuildDecisionTreeTestThrowsFileNotFoundException()
    {
        string path = "./Tree.txt";
        var getDecisionTreeStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetDecisionTree", (object[] args) => getDecisionTreeStrategy.Object.RunStrategy(args)).Execute();
        getDecisionTreeStrategy.Setup(t => t.RunStrategy(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();

        var bdt = new BuildDecisionTree(path);

        Assert.Throws<FileNotFoundException>(() => bdt.execute());

        getDecisionTreeStrategy.Verify();
    }
}
