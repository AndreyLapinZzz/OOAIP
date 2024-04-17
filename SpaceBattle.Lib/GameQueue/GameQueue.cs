namespace SpaceBattle.Lib;

public class GameQueuePushCommand : ICommand
{
    private IQueue<ICommand> queue;
    private ICommand cmd;
    public GameQueuePushCommand(IQueue<ICommand> queue, ICommand cmd)
    {
        this.queue = queue;
        this.cmd = cmd;
    }

    public void execute() {
        queue.Push(cmd);
    }
}
