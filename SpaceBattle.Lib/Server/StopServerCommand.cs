namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    public void execute(){
        //1)��� ����� ��������� � ����� 2)���������� �� ��� ��� 3)������ ������ ���������
        int[] IdsArray = IoC.Resolve<BlockingCollection<int>>("ThreadsIDs").ToArray();
        foreach (int id in IdsArray) {
            //1)��� ����� ��������� � ����� 2)���������� �� ��� ��� 3)������ ������ ���������
            ICommand softStopCommand = IoC.Resolve<ICommand>("Soft Stop The Thread", id);
            try{
                //���� �������� �������� 2) �� ��������
                softStopCommand.execute();
            } catch (Exception e)
            {
                //1)��� ����� ��������� � ����� 2)���������� �� ��� ��� 3)������ ������ ��������� 4) ������ ��������� ������
                IoC.Resolve<ICommand>("ExceptionHandler", e, softStopCommand).execute();
            }
        }
    }
}
