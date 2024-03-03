using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    MyThread stoppingThread;
    public SoftStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.MyStopCommand", stoppingThread)).execute();
    }
}
