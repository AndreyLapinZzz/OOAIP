namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerCommand : ICommand
{
    private int Len;
    public StartServerCommand(int Len){
        this.Len = Len;
    }
    public void execute(){
        for(int i=0; i<Len; i++){
            IoC.Resolve<ICommand>("StartTheadsSrategy", i).execute();
        }

    }

}
