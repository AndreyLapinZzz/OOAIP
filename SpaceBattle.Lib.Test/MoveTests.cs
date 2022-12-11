using Moq;

namespace SpaceBattle.Lib.Test;

public class MoveTests
{
    [Fact]
    public void PositiveMoveObject()
    {
        Mock<IMovable> mockMovable = new();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        new MoveCommand(mockMovable.Object).execute();
        mockMovable.VerifySet(a => a.Pos = new Vector(5, 8));
        mockMovable.Verify();
    }


    [Fact]
    public void CannotGetPosition()
    {
        Mock<IMovable> mockMovable = new();

        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Throws<Exception>();

        MoveCommand c = new MoveCommand(mockMovable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }


    [Fact]
    public void CannotGetVelocity()
    {
        Mock<IMovable> mockMovable = new();

        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Pos).Returns(new Vector(12, 5)).Verifiable();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Throws<Exception>();

        MoveCommand c = new MoveCommand(mockMovable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }


    [Fact]
    public void CannotSetPosition()
    {
        Mock<IMovable> mockMovable = new();
        
        mockMovable.SetupProperty(mockMovable => mockMovable.Pos, new Vector(12, 5));
        mockMovable.SetupSet<Vector>(mockMovable => mockMovable.Pos = It.IsAny<Vector>()).Throws<Exception>();
        mockMovable.SetupGet<Vector>(mockMovable => mockMovable.Velocity).Returns(new Vector(-7, 3)).Verifiable();

        var c = new MoveCommand(mockMovable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }
}
