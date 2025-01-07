namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerCommand : ICommand
{
    private uint Len;
    public StartServerCommand(uint Len){
        //Протестировать если len =-2, =0, =3
        this.Len = Len;
    }
    public void execute(){
        for(int i=0; i<Len; i++){
            IoC.Resolve<ICommand>("StartTheadsStrategy", i).execute(); 
            //1) Если стратегии нет в аёке 2)Если возвращает не ICommand 3)Если комманда выполняется с ошибкой 4) Если всё ок
        }
    }
}
