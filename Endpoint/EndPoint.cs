using Hwdtech;
using CoreWCF;

namespace Endpoint;
/// <summary>
/// Endpoint
/// </summary>
[ServiceBehavior]
public class EndPoint : IEndPoint
{
    /// <summary>
    /// Http Command
    /// </summary>
    public string HttpCommand(Message param)
	{
		var threadID = IoC.Resolve<string>("Game.GetThreadIDByGameID", param.GameID);
		var cmd = IoC.Resolve<Hwdtech.ICommand>("Command.CreateFromMessage", param);
		IoC.Resolve<Hwdtech.ICommand>("Thread.SendCmd", threadID, cmd).Execute();
		return "OK";
	}
}
