namespace SpaceBattle.Lib;

public interface IMoveCommandEndable{
    ICommand com {get;}
    IUObject obj {get;}
    Queue<ICommand> queue {get;}
}
