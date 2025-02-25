using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System.Windows.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Xunit;
using SpaceBattle.Lib;


namespace SpaceBattle.Lib.Test;

public class DeleteGameStrategyTests
{
    public DeleteGameStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    //1)Нут стратегии
    [Fact]
    public void GameCommandDeleteNotInIoCTest()
    {
        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(time));
    }
    //2) Ошибка в стратегии
    [Fact]
    public void ErrorInGameCommandDeleteTest()
    {
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<InvalidOperationException>(() => myStrategy.RunStrategy(time));
        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    //3)Нет аргумента 
    [Fact]
    public void NoArgsGameCommandDeleteTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<IndexOutOfRangeException>(() => myStrategy.RunStrategy());
        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Never());
    }
    //4)Нут стратегии
    [Fact]
    public void DeleteGameScopeNotInIoCTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(time));
        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    //5) Ошибка в стратегии
    [Fact]
    public void ErrorInDeleteGameScopeTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockDeleteGameScopeStrategy = new();
        mockDeleteGameScopeStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteGameScope",
            (object[] args) => mockDeleteGameScopeStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<InvalidOperationException>(() => myStrategy.RunStrategy(time));

        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockDeleteGameScopeStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    //6)Нет аргумента 
 
    //7)Нут стратегии
    [Fact]
    public void BuildMacroCommandNotInIoCTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockDeleteGameScope = new();
        Mock<IStrategy> mockDeleteGameScopeStrategy = new();
        mockDeleteGameScopeStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockDeleteGameScope.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteGameScope",
            (object[] args) => mockDeleteGameScopeStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(time));

        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockDeleteGameScopeStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    //8) Ошибка в стратегии
    [Fact]
    public void ErrorInBuildMacroCommandTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockDeleteGameScope = new();
        Mock<IStrategy> mockDeleteGameScopeStrategy = new();
        mockDeleteGameScopeStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockDeleteGameScope.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteGameScope",
            (object[] args) => mockDeleteGameScopeStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockBuildMacroCommandStrategy = new();
        mockBuildMacroCommandStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "BuildMacroCommand",
            (object[] args) => mockBuildMacroCommandStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        Assert.Throws<InvalidOperationException>(() => myStrategy.RunStrategy(time));

        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockDeleteGameScopeStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockBuildMacroCommandStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    [Fact]
    public void DeleteGameStrategyOKTest()
    {
        Mock<ICommand> mockGameCommandDelete = new();
        Mock<IStrategy> mockGameCommandDeleteStrategy = new();
        mockGameCommandDeleteStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockGameCommandDelete.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Delete",
            (object[] args) => mockGameCommandDeleteStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockDeleteGameScope = new();
        Mock<IStrategy> mockDeleteGameScopeStrategy = new();
        mockDeleteGameScopeStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockDeleteGameScope.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteGameScope",
            (object[] args) => mockDeleteGameScopeStrategy.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockBuildMacroCommand = new();
        Mock<IStrategy> mockBuildMacroCommandStrategy = new();
        mockBuildMacroCommandStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockBuildMacroCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "BuildMacroCommand",
            (object[] args) => mockBuildMacroCommandStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        DeleteGameStrategy myStrategy = new DeleteGameStrategy();
        myStrategy.RunStrategy(time);

        mockGameCommandDeleteStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockDeleteGameScopeStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockBuildMacroCommandStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
}
