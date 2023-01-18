namespace SpaceBattle.Lib;

public interface IMoveCommandEndable{
    MoveCommand com {get;}
    IUObject obj {get;}
    Queue<ICommand> queue {get;}
}
