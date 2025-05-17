using Hwdtech;
using System.Windows.Input;
using CoreWCF;

namespace Endpoint;

[ServiceBehavior]
public class EndPoint : IEndPoint
{
	public string HttpCommand(Message param)
	{
		//1)��� ����� ��������� � ����� 2)������ ������ ���������
		var threadID = IoC.Resolve<string>("Game.GetThreadIDByGameID", param.GameID);
		//3)��� ����� ��������� � ����� 4)������ ������ ���������
		var cmd = IoC.Resolve<Hwdtech.ICommand>("Command.CreateFromMessage", param);
		//5)��� ����� ��������� � ����� 6)���������� �� ��� ��� 7)������ ������ ��������� 8)������������ ��������� 9)������ ������ ����������
		IoC.Resolve<Hwdtech.ICommand>("Thread.SendCmd", threadID, cmd).Execute();
		return "OK";
	}
}
