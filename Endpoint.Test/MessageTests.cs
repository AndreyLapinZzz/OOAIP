using Hwdtech;
using Hwdtech.Ioc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Endpoint.Test;

public class MessageTests
{
    public MessageTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    private readonly HashSet<string> _validTypes = new HashSet<string> { "Type1", "Type2", "Type3" };
    private readonly HashSet<string> _validGameIDs = new HashSet<string> { "GameID1", "GameID2", "GameID3" };
    private readonly HashSet<string> _validGameItemIDs = new HashSet<string> { "ItemID1", "ItemID2", "ItemID3" };
    private readonly HashSet<Dictionary<string, object>> _validProperties = new HashSet<Dictionary<string, object>> 
    {
        new Dictionary<string, object> { { "Property1", 1 } },
        new Dictionary<string, object> { { "Property2", 2 } },
        new Dictionary<string, object> { { "Property3", 3 } }
    }; 

    [Fact]
    public void NotSpeTypeCommandTest()
    {
        var message = new Message();
        var type = message.Type;
        Assert.Null(type);
    }

    [Fact]
    public void NoTypeCommandTest()
    {
        var message = new Message();
        var type = message.Type;
        Assert.DoesNotContain(type, _validTypes);
    }

    [Fact]
    public void NotSpeGameIDTest()
    {
        var message = new Message();
        var gameID = message.GameID;
        Assert.Null(gameID); 
    }

    [Fact]
    public void NoGameIDTest()
    {
        var message = new Message();
        var gameID = message.GameID;
        Assert.DoesNotContain(gameID, _validGameIDs);
    }

    [Fact]
    public void NotSpeGameItemIDTest()
    {
        var message = new Message();
        var gameItemID = message.GameItemID;
        Assert.Null(gameItemID);
    }

    [Fact]
    public void NoGameItemIDTest()
    {
        var message = new Message();
        var gameItemID = message.GameItemID;
        Assert.DoesNotContain(gameItemID, _validGameItemIDs);
    }

    [Fact]
    public void NotSpePropertiesTest()
    {
        var message = new Message();
        var properties = message.Properties;
        Assert.Null(properties);
    }

    [Fact]
    public void IncorrectPropertiesTest()
    {
        var message = new Message();
        var properties = message.Properties;
        Assert.DoesNotContain(properties, _validProperties);
    }
}
