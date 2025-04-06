using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Collections;

namespace SpaceBattle.Lib.Test;

public class CreateGameScopeStrategyTests
{
    public CreateGameScopeStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void ArgsIsEmptyTest()
    {
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<IndexOutOfRangeException>(() => myStrategy.RunStrategy());
    }
 
    [Fact]
    public void ServerGameIDNewStrategyNotInIoCTest()
    {
        object[] args = new object[] { 7 };
        CreateGameScopeStrategy myStrategy = new CreateGameScopeStrategy();
        Assert.Throws<ArgumentException>(() => myStrategy.RunStrategy(args));
    }
  
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
        Assert.Empty(queue);
    }

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
        Assert.Empty(entities);
    }
    
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

        Assert.Throws<IndexOutOfRangeException>(() =>  IoC.Resolve<ICommand>("Enqueue"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }

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

        Assert.Throws<InvalidCastException>(() => IoC.Resolve<ICommand>("Enqueue", 2));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

        mockNewGameID.Verify(strategy => strategy.RunStrategy(It.IsAny<object[]>()), Times.Exactly(1));
    }
    
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
        Assert.Single(queue);
    }

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
        Assert.Single(queue);
    }

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
        Assert.Single(queue);
        Assert.Equal(mockICommand1.Object, result);
    }

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
        Assert.Single(queue);
    }

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
        Assert.Single(ent);
    }

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
        Assert.Single(ent);
    }

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
