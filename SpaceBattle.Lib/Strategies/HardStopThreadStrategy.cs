namespace SpaceBattle.Lib;

public class HardStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread stoppingThread = (MyThread) args[0];
        
        //thread.Stop();
        ICommand hardStopCommand = new HardStopCommand(stoppingThread);

        return hardStopCommand;
        //return new Command();
    }
}