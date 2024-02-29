using Hwdtech;
namespace SpaceBattle.Lib;

public class SendCommand : ICommand
{
    MyThread thread;
    ICommand cmd;

    public SendCommand(MyThread thread, ICommand cmd)
    {
        this.thread = thread;
        this.cmd = cmd;
    }

    public void execute()
    {
        thread.queue.Push(cmd);
    }
}
