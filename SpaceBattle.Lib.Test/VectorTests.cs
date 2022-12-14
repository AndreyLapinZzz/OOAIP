using Moq;

namespace SpaceBattle.Lib.Test;

public class VectorTests {

    [Fact]
    public void VectorGetIndex()
    {
        var a = new Vector(1, 2, 3);

        Assert.True(a[0] == 1);
    }

    [Fact]
    public void VectorSetIndex()
    {
        var a = new Vector(3, 4);
        a[1] = 1;

        Assert.True(a[1] == 1);
    }

    [Fact]
    public void SumError()
    {
        Vector a = new Vector(0,0,0,0);
        Vector b = new Vector(0,0,0);
        Assert.Throws<ArgumentException>(() => a + b);
    }

    [Fact]
    public void SumPositive()
    {
        Vector a = new Vector(1,0,3,0);
        Vector b = new Vector(0,2,0,4);
        Assert.True(new Vector(1,2,3,4) == a + b);
    }

    [Fact]
    public void VectorEqualError()
    {
        Vector a = new Vector(0,0,0,0);
        Vector b = new Vector(0,0,0);
        Assert.False(a == b);
    }

    [Fact]
    public void EqualsNegative()
    {
        Vector a = new Vector(0, 1);
        int b = 1;
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void PositiveVectorEqual()
    {
        Vector a = new Vector(0,1);
        Vector b = new Vector(0,1);
        Assert.True(a == b);
    }

    [Fact]
    public void NotEqualPositive()
    {
        Vector a = new Vector(0,0);
        Vector b = new Vector(0,1);
        Assert.True(a != b);
    }

    [Fact]
    public void VectorHashCodeEqual(){
        Vector a = new Vector(0,1,1,0);
        Vector b = new Vector(0,1,1,0);
        Assert.True(a.GetHashCode() == b.GetHashCode());
    }
}
