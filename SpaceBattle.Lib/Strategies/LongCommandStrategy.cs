using Hwdtech;

namespace SpaceBattle.Lib;

public class LongCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args){
        var nameDepend = (string)args[0];
        IUObject obj = (IUObject)args[1];
        var comA = IoC.Resolve<RepeatCommand>("Game.CreateMacroCommand", nameDepend, obj);
        var repCom = IoC.Resolve<RepeatCommand>("Game.CommandRepeat", comA);
        return IoC.Resolve<RepeatCommand>("Game.QueuePushBack", repCom);
    }
}
