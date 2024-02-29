using Hwdtech;
using System.Collections.Concurrent;
namespace SpaceBattle.Lib;

public class ReceiveAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;
    public ReceiveAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.LongCount() == 0;
    }
    
    public void Push(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
