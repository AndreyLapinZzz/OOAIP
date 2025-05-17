namespace SpaceBattle.Lib;
using Hwdtech;

public class StopServerStrategy : IStrategy
{
    public object RunStrategy(params object[] args){
        return new StopServerCommand();
        //3)Всё ок 
    }
}
