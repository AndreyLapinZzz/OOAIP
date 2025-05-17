using Hwdtech;
using System.Collections;

namespace SpaceBattle.Lib;

public class CreateGameScopeStrategy : IStrategy
{
	public object RunStrategy(params object[] args)
	{
		object newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
		object oldScope = IoC.Resolve<object>("Scopes.Current");
		object quantumTime = args[0];
		Queue<ICommand> queue = new Queue<ICommand>();
		Dictionary<string, IUObject> entities = new();
		string idGame = IoC.Resolve<string>("Server.GameID.New");

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameID",
			(object[] p) => idGame
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue",
			(object[] p) => queue
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObjects",
			(object[] p) => entities
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantumTime",
			(object[] p) => quantumTime
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Enqueue",
			(object[] p) => {
				queue.Enqueue((SpaceBattle.Lib.ICommand)p[0]);
				return (object)true;
			}
		).Execute();
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dequeue",
			(object[] p) => {
				return (object)queue.Dequeue();
			}
		).Execute();
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObject",
			(object[] p) => (object)entities[(string)p[0]]
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RemoveObject",
			(object[] p) => (object)entities.Remove((string)p[0])
		).Execute();

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

		return newScope;
	}
}
