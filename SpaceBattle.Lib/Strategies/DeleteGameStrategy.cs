using Hwdtech;
using System.IO;
using System.Collections;
using System.Collections.Concurrent;
using System.Windows.Input;

namespace SpaceBattle.Lib;

public class DeleteGameStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        //1)Нут стратегии 2) Ошибка в стратегии 3)Нет аргумента 
        ICommand cmd1 = IoC.Resolve<ICommand>("GameCommand.Delete", args[0]);
        //4)Нут стратегии 5) Ошибка в стратегии 6)Нет аргумента 
        ICommand cmd2 = IoC.Resolve<ICommand>("DeleteGameScope", args[0]);
        //7)Нут стратегии 8) Ошибка в стратегии
        ICommand macroCmd = IoC.Resolve<ICommand>("BuildMacroCommand", cmd1, cmd2);
        return macroCmd;
    }
}
