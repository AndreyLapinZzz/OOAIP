namespace SpaceBattle.Lib;
using Hwdtech;

public class SoftStopTheThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread stoppingThread = (MyThread) args[0];
        
        ICommand cmd = IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.HardStopTheThread", stoppingThread)); // отправляем в очередь HardStop

        return cmd;
    }
}