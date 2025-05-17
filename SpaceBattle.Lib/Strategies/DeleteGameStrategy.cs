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
        //1)��� ��������� 2) ������ � ��������� 3)��� ��������� 
        ICommand cmd1 = IoC.Resolve<ICommand>("GameCommand.Delete", args[0]);
        //4)��� ��������� 5) ������ � ��������� 6)��� ��������� 
        ICommand cmd2 = IoC.Resolve<ICommand>("DeleteGameScope", args[0]);
        //7)��� ��������� 8) ������ � ���������
        ICommand macroCmd = IoC.Resolve<ICommand>("BuildMacroCommand", cmd1, cmd2);
        return macroCmd;
    }
}
