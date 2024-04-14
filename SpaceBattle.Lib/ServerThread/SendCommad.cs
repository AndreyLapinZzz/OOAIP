using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SendCommand : ISender
{
    BlockingCollection<ICommand> queue;

    public SendCommand(BlockingCollection<ICommand> queue)
    {
        this.queue = queue;
    }
    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
