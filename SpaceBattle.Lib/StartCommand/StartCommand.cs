using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    private IMoveCommandStartable obj;
    public StartMoveCommand(IMoveCommandStartable obj)
    {
        this.obj = obj;
    }

    public void execute()
    {
        obj.properties.ToList().ForEach(o => IoC.Resolve<ICommand>("SpaceBattle.SetProperty", obj.uobject, o.Key, o.Value).execute());
        var moveCmd = IoC.Resolve<ICommand>("SpaceBattle.Commands.Move", obj.uobject);
        IoC.Resolve<ICommand>("SpaceBattle.SetProperty", obj.uobject, "Move", moveCmd).execute();
        IoC.Resolve<ICommand>("SpaceBattle.QueuePush", moveCmd).execute();
    }
}
