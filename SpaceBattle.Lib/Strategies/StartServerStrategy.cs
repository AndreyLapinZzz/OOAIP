namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerStrategy : IStrategy
{
    public object RunStrategy(params object[] args){
        return new StartServerCommand((int)args[0]);
    }

}
