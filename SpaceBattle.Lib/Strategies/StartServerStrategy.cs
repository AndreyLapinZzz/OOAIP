namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerStrategy : IStrategy
{
    public object RunStrategy(params object[] args){
        return new StartServerCommand((uint)args[0]);
        //1)Если аргс0 не аинт,2)Если нет параметра, 3)Всё ок 
    }
}
