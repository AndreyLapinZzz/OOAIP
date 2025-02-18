namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerCommand : ICommand
{
    private uint Len;
    public StartServerCommand(uint Len){
        //�������������� ���� len =-2, =0, =3
        this.Len = Len;
    }
    public void execute(){
        for(int i=0; i<Len; i++){
            IoC.Resolve<ICommand>("StartTheadsStrategy", i).execute(); 
            //1) ���� ��������� ��� � ��� 2)���� ���������� �� ICommand 3)���� �������� ����������� � ������� 4) ���� �� ��
        }
    }
}
