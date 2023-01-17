using Hwdtech;

namespace SpaceBattle.Lib;

public class LongCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args){
        var nameDepend = (string)args[0];
        IUObject obj = (IUObject)args[1];
        var comA = IoC.Resolve<ICommand>("Game.CreateMacroCommand", nameDepend, obj);
        var repCom = IoC.Resolve<ICommand>("Game.CommandRepeat", comA);
        return IoC.Resolve<ICommand>("Game.QueuePushBack", repCom);
    }
}
