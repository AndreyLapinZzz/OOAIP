using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class DeleteGameStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        ICommand cmd1 = IoC.Resolve<ICommand>("GameCommand.Delete", args[0]);
        ICommand cmd2 = IoC.Resolve<ICommand>("DeleteGameScope", args[0]);
        ICommand macroCmd = IoC.Resolve<ICommand>("BuildMacroCommand", cmd1, cmd2);
        return macroCmd;
    }
}
