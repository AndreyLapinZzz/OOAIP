namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    MyThread stoppingThread;
    public SoftStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;
    public void execute()
    {
        var cmd = new UpdateBehaviourCommand
            (
                stoppingThread,
                () =>
                {
                    if (stoppingThread.queue.isEmpty())
                    {
                        stoppingThread.Stop();
                    }
                    else
                    {
                        stoppingThread.HandleCommand();
                    }
                }
            );
        cmd.execute();
    }
}
