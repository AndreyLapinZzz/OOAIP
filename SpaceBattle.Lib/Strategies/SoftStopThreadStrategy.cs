namespace SpaceBattle.Lib;
using Hwdtech;

public class SoftStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread stoppingThread = (MyThread) args[0];
        ICommand softStopCommand = new SoftStopCommand(stoppingThread);

        return softStopCommand;
    }
}
