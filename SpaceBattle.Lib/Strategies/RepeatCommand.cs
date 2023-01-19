namespace SpaceBattle.Lib;

public class RepeatCommand : ICommand
{
    public List<ICommand> comList = new List<ICommand>();
    public RepeatCommand(ICommand obj){
        comList.Add(obj);
    }
    public void execute(){   
        comList[0].execute();
        comList.Add(comList[0]);
        comList.RemoveAt(0);
    }
}