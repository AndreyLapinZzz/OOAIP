namespace SpaceBattle.Lib;
using Hwdtech;

public class SoftStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread stoppingThread = (MyThread) args[0];

        ICommand softStopCommand = new SoftStopCommand(stoppingThread);

        return softStopCommand;
        
        // ICommand cmd = IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.HardStopTheThread", stoppingThread)); // отправляем в очередь HardStop

        // return cmd;
    }
}