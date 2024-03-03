using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    MyThread stoppingThread;
    public SoftStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        stoppingThread.Stop();
    }
}
