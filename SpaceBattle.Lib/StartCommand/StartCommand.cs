using Hwdtech;

namespace SpaceBattle.Lib;

public class StartCommand : ICommand
{
    private IStartable obj;
    public StartCommand(IStartable obj)
    {
        this.obj = obj;
    }

    public void execute()
    {
        obj.properties.ToList().ForEach(o => IoC.Resolve<ICommand>("Game.SetProperty", obj.uobject, o.Key, o.Value).execute());
        var moveCmd = IoC.Resolve<ICommand>("Game.Commands.Move", obj.uobject);
        IoC.Resolve<ICommand>("Game.SetProperty", obj.uobject, "Move", moveCmd).execute();
        IoC.Resolve<ICommand>("Game.QueuePush", moveCmd).execute();
    }
}
