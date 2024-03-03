using Hwdtech;

namespace SpaceBattle.Lib;

public class MyStopCommand : ICommand {
    MyThread stoppingThread;
    public MyStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        IoC.Resolve<ICommand>("Game.SendCommand", stoppingThread, IoC.Resolve<ICommand>("Game.HardStopThreadStrategy", stoppingThread)).execute();
    }
}
