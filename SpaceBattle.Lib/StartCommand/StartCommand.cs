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
        ICommand moveCommand = IoC.Resolve<ICommand>("Command.Move", obj.uobject);
        IoC.Resolve<ICommand>("Game.SetProperty", obj.uobject, "Commands.Movement", moveCommand).execute();
        IoC.Resolve<ICommand>("Queue.Push", moveCommand).execute();
    }
}
