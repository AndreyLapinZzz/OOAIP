using Moq;

namespace SpaceBattle.Lib.Test;

public class RotateTests
{
    [Fact]
    public void RotateObjectPositive()
    {
        Mock<IRotable> mockRoteble = new();
        mockRoteble.SetupGet<Ugle>(mockRoteble => mockRoteble.Angle).Returns(new Ugle(45, 1)).Verifiable();
        mockRoteble.SetupGet<Ugle>(mockRoteble => mockRoteble.AngleVelocity).Returns(new Ugle(90, 1)).Verifiable();

        new RotateCommand(mockRoteble.Object).execute();
        mockRoteble.VerifySet(a => a.Angle = new Ugle(135, 1));
        mockRoteble.Verify();
    }

    [Fact]
    public void RotateObjectNegative()
    {
        Mock<IRotable> mockRoteble = new();
        mockRoteble.SetupGet<Ugle>(mockRoteble => mockRoteble.Angle).Returns(new Ugle(-90, 2)).Verifiable();
        mockRoteble.SetupGet<Ugle>(mockRoteble => mockRoteble.AngleVelocity).Returns(new Ugle(-450, -1)).Verifiable();

        new RotateCommand(mockRoteble.Object).execute();
        mockRoteble.VerifySet(a => a.Angle = new Ugle(45, 1));
        mockRoteble.Verify();
    }

    [Fact]
    public void CannotGetAngle()
    {
        Mock<IRotable> mockRotable = new();

        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.Angle).Returns(new Ugle(45, 1)).Verifiable();
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.AngleVelocity).Returns(new Ugle(90, 1)).Verifiable();
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.Angle).Throws<Exception>();

        RotateCommand c = new RotateCommand(mockRotable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }

    [Fact]
    public void AngleEqualsNull()
    {
        Mock<IRotable> mockRotable1 = new();
        mockRotable1.SetupGet<Ugle>(mockRotable1 => mockRotable1.Angle).Returns(new Ugle(90, 45)).Verifiable();

        Assert.False(mockRotable1.Object.Angle.Equals(null));
    }

    [Fact]
    public void AngleEquals()
    {
        Mock<IRotable> mockRotable1 = new();
        Mock<IRotable> mockRotable2 = new();
        mockRotable1.SetupGet<Ugle>(mockRotable1 => mockRotable1.Angle).Returns(new Ugle(90, 1)).Verifiable();
        mockRotable2.SetupGet<Ugle>(mockRotable2 => mockRotable2.Angle).Returns(new Ugle(90, 1)).Verifiable();

        Assert.True(mockRotable1.Object.Angle.Equals(mockRotable2.Object.Angle));
    }

    [Fact]
    public void AngleEqualsItSelf()
    {
        Mock<IRotable> mockRotable1 = new();
        mockRotable1.SetupGet<Ugle>(mockRotable1 => mockRotable1.Angle).Returns(new Ugle(90, 45)).Verifiable();

        Assert.True(mockRotable1.Object.Angle.Equals(mockRotable1.Object.Angle));
    }

    [Fact]
    public void AngleEqualsD()
    {
        Mock<IRotable> mockRotable1 = new();
        Mock<IRotable> mockRotable2 = new();
        mockRotable1.SetupGet<Ugle>(mockRotable1 => mockRotable1.Angle).Returns(new Ugle(90, 45)).Verifiable();
        mockRotable2.SetupGet<Ugle>(mockRotable2 => mockRotable2.Angle).Returns(new Ugle(90, 2)).Verifiable();

        Assert.False(mockRotable1.Object.Angle.Equals(mockRotable2.Object.Angle));
    }

    
    [Fact]
    public void CannotGetVelocity()
    {
        Mock<IRotable> mockRotable = new();

        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.Angle).Returns(new Ugle(45, 1)).Verifiable();
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.AngleVelocity).Throws<Exception>();

        RotateCommand c = new RotateCommand(mockRotable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }

    [Fact]
    public void CannotGetVelocity2()
    {
        Mock<IRotable> mockRotable = new();
    
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.Angle).Returns(new Ugle()).Verifiable();
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.AngleVelocity).Returns(new Ugle(90, 2)).Verifiable();

        RotateCommand c = new RotateCommand(mockRotable.Object);

        Assert.Throws<ArgumentException>(() => c.execute());
    }


    [Fact]
    public void CannotSetAngle()
    {
        Mock<IRotable> mockRotable = new();
        
        mockRotable.SetupProperty(mockRotable => mockRotable.Angle, new Ugle(45, 1));
        mockRotable.SetupSet<Ugle>(mockRotable => mockRotable.Angle = It.IsAny<Ugle>()).Throws<Exception>();
        mockRotable.SetupGet<Ugle>(mockRotable => mockRotable.AngleVelocity).Returns(new Ugle(90, 1)).Verifiable();

        var c = new RotateCommand(mockRotable.Object);

        Assert.Throws<Exception>(() => c.execute());
    }
}
