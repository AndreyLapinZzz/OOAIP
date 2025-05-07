namespace SpaceBattle.Lib;

public class RepeatCommand : ICommand
{
    public List<ICommand> comList;
    public RepeatCommand(List<ICommand> obj){
        comList = obj;
    }
    public void execute(){   
        comList[0].execute();
        comList.RemoveAt(0);
    }
}