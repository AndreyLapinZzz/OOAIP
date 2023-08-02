namespace SpaceBattle.Lib;

public class SomeStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        int a = (int)args[0];
        int b = (int)args[1];
        int c = a + b;

        return c;
    }
}