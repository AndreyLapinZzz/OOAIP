namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    MyThread stoppingThread;
    public HardStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        stoppingThread.Stop();
    }
}
