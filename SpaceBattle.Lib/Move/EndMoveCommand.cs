namespace SpaceBattle.Lib;
using Hwdtech;

public class EndMoveCommand : ICommand
{
    IMoveCommandEndable com;
    public EndMoveCommand(IMoveCommandEndable com){
        this.com = com;
    }
    public void execute(){
        IoC.Resolve<ICommand>(
            "Game.Command.DeleteProperty",
            com.obj,
            "Move"
        ).execute();

        IoC.Resolve<ICommand>(
            "Game.Command.Inject",
            com.com,
            IoC.Resolve<ICommand>("Game.Command.Empty")
        ).execute();
    }
}
