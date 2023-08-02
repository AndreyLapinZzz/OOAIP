// namespace SpaceBattle.Lib.Test;
// using Moq;
// using Hwdtech;
// using Hwdtech.Ioc;

// public class MultiThreadedStategulServerTests
// {
//     public MultiThreadedStategulServerTests()
//     {
//         new InitScopeBasedIoCImplementationCommand().Execute();
//         IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

//         var CreateAndStartThreadStrategy = new CreateAndStartThreadStrategy();
//         IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CreateAndStartThread", (object[] args) => CreateAndStartThreadStrategy.RunStrategy(args)).Execute();
//     }

//     [Fact]
//     public void ServerStartCheck()
//     {
//         IoC.Resolve<ICommand>("Game.CreateAndStartThread").Execute();


//         //var cmd = new Mock<ICommand>();
//         //cmd.Setup(c => c.Execute()).Callback(cv => cv.Notify).Verifiable();
//     }
    
//     [Fact]
//     public void CheckingExecutionOfCommandFromThread()
//     {
//         //IoC.Resolve<ICommand>("Game.CreateAndStartThread").Execute();
//     }
// }