using System.Runtime.CompilerServices;

namespace SpaceBattle.Lib;

public class HardStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread stoppingThread = (MyThread) args[0];
        stoppingThread.Stop();

        return new HardStopCommand(stoppingThread);
    }
}
