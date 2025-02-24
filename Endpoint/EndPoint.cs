using Hwdtech;
using System.Windows.Input;
using CoreWCF;

namespace Endpoint;

[ServiceBehavior]
public class EndPoint : IEndPoint
{
	public string HttpCommand(Message param)
	{
		//1)Нет такой стратегии в айоке 2)Ошибка внутри стратегии
		var threadID = IoC.Resolve<string>("Game.GetThreadIDByGameID", param.GameID);
		//3)Нет такой стратегии в айоке 4)Ошибка внутри стратегии
		var cmd = IoC.Resolve<Hwdtech.ICommand>("Command.CreateFromMessage", param);
		//5)Нет такой стратегии в айоке 6)Возвращает не тот тип 7)Ошибка внутри стратегии 8)Некорректные параметры 9)ошибка внутри айкомманда
		IoC.Resolve<Hwdtech.ICommand>("Thread.SendCmd", threadID, cmd).Execute();
		return "OK";
	}
}
