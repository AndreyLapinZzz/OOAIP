namespace SpaceBattle.Lib;
using Hwdtech;
using System.IO;
using System.Collections;

public class WriteExeptionCommand : ICommand
{
    private Exception ex;
    public WriteExeptionCommand(Exception ex)
    {
        this.ex = ex;
    }
    public void execute()
    {
        //1)��� ����� ��������� � ����� 2)���������� �� ��� ��� 3)������ ������ ���������
        IEnumerable<object> info = IoC.Resolve<IEnumerable<object>>("GetInfoExeption", ex);
        //1)��� ����� ��������� � ����� 2)���������� �� ��� ��� 3)������ ������ ���������
        using (TextWriter myOut = IoC.Resolve<TextWriter>("GetLogStream")){
            //1)���� ������ ������ ��������� 2)�� ��
            info.ToList().ForEach (item => myOut.WriteLine(item) );
        }
    }
}
