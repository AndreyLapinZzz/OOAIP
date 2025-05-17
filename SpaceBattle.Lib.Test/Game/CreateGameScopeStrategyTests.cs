using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using Xunit;
using SpaceBattle.Lib;
using System.Collections;

namespace SpaceBattle.Lib.Test;

public class CreateGameScopeStrategyTests
{
    public CreateGameScopeStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        //IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ExceptionHandler", (object[] args) => exceptionHandler.Object.RunStrategy(args)).Execute();
    }

    //1)аргс пустой
    [Fact]
    public void ArgsIsEmptyTest()
    {
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<IndexOutOfRangeException>(() => myStrategy.RunStrategy());
    }
    
    //2)Нет стратегии
    [Fact]
    public void ServerGameIDNewStrategyNotInIoCTest()
    {
        object[] args = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(args));
    }
    
    //3)Ошибка в стратегии
    [Fact]
    public void ErrorInServerGameIDNewStrategyTest()
    {
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Throws<InvalidOperationException>().Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] args = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<InvalidOperationException>(() => myStrategy.RunStrategy(args));
        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    //4)Возвращает не тот тип
    [Fact]
    public void NotStringServerGameIDNewStrategyTest()
    {
        var Any = new object();
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(Any).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] args = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<InvalidCastException>(() => myStrategy.RunStrategy(args));
        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    //5)Проверить что получено idGame 
    [Fact]
    public void ReceivedGameIDTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        string idGame = IoC.Resolve<string>("GetGameID");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(newId, idGame);
    }
    
    //6)Проверить что очередь пустая
    [Fact]
    public void QueueEmptyTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(0, queue.Count);
    }
    
    //7)Проверить что словарь пустой
    [Fact]
    public void EntitiesEmptyTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Dictionary<string, IUObject> entities = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(0, entities.Count);
    }
    
    //8)Проверить что получено  quantumTime
    [Fact]
    public void ReceivedQuantumTimeTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        object quantumTime = IoC.Resolve<object>("GetQuantumTime");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(time[0], quantumTime);
    }
    
    //9)Список аргументов пустой
    [Fact]
    public void EnqueueArgsEmptyTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        ICommand result = null;
        Assert.Throws<IndexOutOfRangeException>(() => result = IoC.Resolve<ICommand>("Enqueue"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Null(result);
    }

    //10)Если первый аргумент не команда
    [Fact]
    public void EnqueueFirstArgNotCommandTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        ICommand result = null;
        Assert.Throws<InvalidCastException>(() => result = IoC.Resolve<ICommand>("Enqueue", 2));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Null(result);
    }
    
    //11)Есть лишние параметры
    [Fact]
    public void EnqueueExtraParametersTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");

        Mock<ICommand> mockICommand1 = new();
        Mock<ICommand> mockICommand2 = new();

        IoC.Resolve<bool>("Enqueue", mockICommand1.Object, mockICommand2.Object);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(1, queue.Count);
    }
    
    //12)Всё ок, очередь пополнилась
    [Fact]
    public void EnqueueOKTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");
        Mock<SpaceBattle.Lib.ICommand> mockICommand1 = new();
        object[] Com1 = new object[] { mockICommand1.Object };

        bool result = IoC.Resolve<bool>("Enqueue", Com1);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(1, queue.Count);
    }
    
    //15)Есть лишние параметры
    [Fact]
    public void Test()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);
        
        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();


        Mock<ICommand> mockICommand1 = new();
        Mock<ICommand> mockICommand2 = new();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");
        queue.Enqueue(mockICommand1.Object);
        queue.Enqueue(mockICommand2.Object);

        ICommand result = IoC.Resolve<ICommand>("Dequeue", 2, 3, "eivbiq");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(1, queue.Count);
        Assert.Equal(mockICommand1.Object, result);
    }
    
    //16)Нет команды в очереди
    [Fact]
    public void DequeueNoCommandInQueueTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Dequeue"));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
    //17)Очередь стала меньше
    [Fact]
    public void DequeueStrategyIsCorrectTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);
        
        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();


        Mock<ICommand> mockICommand1 = new();
        Mock<ICommand> mockICommand2 = new();

        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");
        queue.Enqueue(mockICommand1.Object);
        queue.Enqueue(mockICommand2.Object);
        ICommand result = IoC.Resolve<ICommand>("Dequeue");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(mockICommand1.Object, result);
        Assert.Equal(1, queue.Count);
    }

    //18)Список аргументов пустой
    [Fact]
    public void GetObjectArgsEmptyTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>> ("GetObjects");
        ent.Add("1", obj.Object);
        Assert.Throws<IndexOutOfRangeException>(() => IoC.Resolve<object>("GetObject"));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //19)Если первый аргумент не строчка
    [Fact]
    public void GetObjectFirstArgNotStringTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        object[] obj1 = new object[] { new object() };
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");
        ent.Add("1", obj.Object);
        Assert.Throws<InvalidCastException>(() => IoC.Resolve<object>("GetObject", obj1));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //20)Есть лишние параметры
    [Fact]
    public void GetObjectExtraParametersTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");
        ent.Add("1", obj.Object);
        object result = IoC.Resolve<object>("GetObject", "1", "2");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));

        Assert.Equal(obj.Object, result);
    }
    
    //21)Всё ок
    [Fact]
    public void GetObjectOKTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        object[] obj1 = new object[] { "1" };
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>> ("GetObjects");
        ent.Add("1", obj.Object);
        object result = IoC.Resolve<object>("GetObject", obj1);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(obj.Object, result);
    }

    //22)Список аргументов пустой
    [Fact]
    public void RemoveObjectArgsEmptyTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        object[] objs1 = new object[] { };
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");

        Assert.Throws<IndexOutOfRangeException>(() => IoC.Resolve<object>("RemoveObject", objs1));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //23)Если первый аргумент не строчка
    [Fact]
    public void RemoveObjectFirstArgNotStringTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        object[] objs1 = new object[] { new object() };
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");

        Assert.Throws<InvalidCastException>(() => IoC.Resolve<object>("RemoveObject", objs1));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

    //24)Есть лишние параметры
    [Fact]
    public void RemoveObjectExtraParametersTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        Mock<IUObject> obj2 = new();
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");
        ent.Add("1", obj.Object);
        ent.Add("2", obj2.Object);
        var obj2L = new List<IUObject> { obj2.Object };

        bool result = IoC.Resolve<bool>("RemoveObject", "1", "2");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));

        Assert.True(result);
        Assert.Equal(obj2L, ent.Values);
        Assert.Equal(1, ent.Count);
    }
    
    //25)Нет такого объекта
    [Fact]
    public void RemoveObjectNoObjectTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");

        bool result = IoC.Resolve<bool>("RemoveObject", "1");

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.False(result);
    }
    
    //26)Объект удалён
    [Fact]
    public void RemoveObjectOKTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

        Mock<IUObject> obj = new();
        Mock<IUObject> obj2 = new();
        object[] obj1 = new object[] { "1" };
        Dictionary<string, IUObject> ent = IoC.Resolve<Dictionary<string, IUObject>>("GetObjects");
        ent.Add("1", obj.Object);
        ent.Add("2", obj2.Object);

        var obj2L = new List<IUObject> { obj2.Object };

        object result = IoC.Resolve<object>("RemoveObject", obj1);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
        Assert.Equal(true, result);
        Assert.Equal(obj2L, ent.Values);
        Assert.Equal(1, ent.Count);
    }

    //27)Старый скоп вернулся
    [Fact]
    public void OldScopeReturnedTest()
    {
        string newId = "236";
        Mock<IStrategy> mockNewGameID = new();
        mockNewGameID.Setup(
            strategy => strategy.RunStrategy(It.IsAny<object[]>())
        ).Returns(newId).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.GameID.New",
            (object[] args) => mockNewGameID.Object.RunStrategy(args)
        ).Execute();

        object[] time = new object[] { 7 };
        object oldScope = IoC.Resolve<object>("Scopes.Current");
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        object newScope = myStrategy.RunStrategy(time);

        object currentScope = IoC.Resolve<object>("Scopes.Current");

        Assert.Equal(oldScope, currentScope);
    }
    
}
