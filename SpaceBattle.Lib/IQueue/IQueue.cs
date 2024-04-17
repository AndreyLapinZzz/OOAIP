namespace SpaceBattle.Lib;

public interface IQueue<T>
{
    public List<ICommand> queue { get; set; }

    public void Push(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
