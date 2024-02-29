using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class CreateAndStartThreadCommand : ICommand
{
    MyThread thread;
    public CreateAndStartThreadCommand(MyThread thread) => this.thread = thread;
    public void execute()
    {
        thread.execute();
    }
}
