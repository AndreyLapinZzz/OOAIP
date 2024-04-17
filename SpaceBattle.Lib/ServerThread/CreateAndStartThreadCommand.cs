namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;


public class CreateAndStartThreadCommand : ICommand
{
    string thread_id;
    public CreateAndStartThreadCommand(string thread_id) => this.thread_id = thread_id;
    public void execute()
    {
        BlockingCollection<ICommand> queue = new BlockingCollection<ICommand>(1000);
        var reciever = new ReceiveAdapter(queue);
        MyThread thread = new MyThread(reciever);
        thread.execute();
        var threads = IoC.Resolve<Dictionary<string, MyThread>>("GetThreadList");
        threads.Add(thread_id, thread);
        var senders = IoC.Resolve<Dictionary<string, ISender>>("GetSenderList");
        var send = new SendCommand(queue);
        senders.Add(thread_id, send);
    }
}
