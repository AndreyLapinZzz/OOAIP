namespace SpaceBattle.Lib;

public class GameQueuePushCommand : ICommand
{
    private Queue<ICommand> queue;
    private ICommand cmd;
    public GameQueuePushCommand(Queue<ICommand> queue, ICommand cmd)
    {
        this.queue = queue;
        this.cmd = cmd;
    }

    public void execute() {
        queue.Enqueue(cmd);
    }
}
