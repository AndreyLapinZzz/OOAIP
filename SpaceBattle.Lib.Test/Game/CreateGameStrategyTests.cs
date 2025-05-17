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

public class CreateGameStrategyTests
{
    public CreateGameStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }
    
    //1)Ошибка внутри стратегии
    [Fact]
    public void ErrorInGameCommandCreateTest()
    {
        CreateGameScopeStrategy createScope = new CreateGameScopeStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGameScopeStrategy",
            (object[] args) => createScope.RunStrategy(args)
        ).Execute();

        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<IStrategy> mockCreateGameCommandStrategy = new();
        mockCreateGameCommandStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Create",
            (object[] args) => mockCreateGameCommandStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameStrategy myStrategy = new CreateGameStrategy();
        Assert.Throws<InvalidOperationException>(() => myStrategy.RunStrategy(time));
        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCreateGameCommandStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    //2)Нет стратегии
    [Fact]
    public void GameCommandCreateNotInIoCTest()
    {
        CreateGameScopeStrategy createScope = new CreateGameScopeStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGameScopeStrategy",
            (object[] args) => createScope.RunStrategy(args)
        ).Execute();

        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameStrategy myStrategy = new CreateGameStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(time));
        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //3)Ок
    [Fact]
    public void ArgsIsEmptyTest()
    {
        CreateGameScopeStrategy createScope = new CreateGameScopeStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGameScopeStrategy",
            (object[] args) => createScope.RunStrategy(args)
        ).Execute();

        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        Mock<ICommand> mockCreateGameCommand = new();
        Mock<IStrategy> mockCreateGameCommandStrategy = new();
        mockCreateGameCommandStrategy.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(mockCreateGameCommand.Object).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand.Create",
            (object[] args) => mockCreateGameCommandStrategy.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameStrategy myStrategy = new CreateGameStrategy();
        myStrategy.RunStrategy(time);
        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        mockCreateGameCommandStrategy.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
}
