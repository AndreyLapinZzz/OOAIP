namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections;

public class StartServerCommand : ICommand
{
    private uint Len;
    public StartServerCommand(uint Len){
        this.Len = Len;
    }
    public void execute(){
        var ids = IoC.Resolve<IEnumerable<uint>>("GenerateThreadIds", Len);
        foreach (var id in ids){
            IoC.Resolve<ICommand>("StartTheadsStrategy", id).execute(); 
        }
    }
}
