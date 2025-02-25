using Hwdtech;
using System.IO;
using System.Collections;

namespace SpaceBattle.Lib;

public class CreateGameScopeStrategy : IStrategy
{
	public object RunStrategy(params object[] args)
	{
		object newScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
		object oldScope = IoC.Resolve<object>("Scopes.Current");
		//1)���� ������
		object quantumTime = args[0];
		Queue<ICommand> queue = new Queue<ICommand>();
		Dictionary<string, IUObject> entities = new();
		//2)��� ��������� 3)������ � ��������� 4)���������� �� ��� ���
		string idGame = IoC.Resolve<string>("Server.GameID.New");

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

		//5)��������� ��� �������� idGame 
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameID",
			(object[] p) => idGame
		).Execute();
		//6)��������� ��� ������� ������
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue",
			(object[] p) => queue
		).Execute();
		//7)��������� ��� ������� ������
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObjects",
			(object[] p) => entities
		).Execute();
		//8)��������� ��� ��������  quantumTime
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantumTime",
			(object[] p) => quantumTime
		).Execute();
		//9)������ ���������� ������ 10)���� ������ �������� �� ������� 11)���� ������ ��������� 12)�� ��, ������� �����������
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Enqueue",
			(object[] p) => {
				queue.Enqueue((SpaceBattle.Lib.ICommand)p[0]);
				return (object)true;
			}
		).Execute();
		//15)���� ������ ��������� 16)��� ������� � ������� 17)������� ����� ������
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dequeue",
			(object[] p) => {
				return (object)queue.Dequeue();
			}
		).Execute();
		
		//18)������ ���������� ������ 19)���� ������ �������� �� ������� 20)���� ������ ��������� 21)�� ��
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObject",
			(object[] p) => (object)entities[(string)p[0]]
		).Execute();
		//22)������ ���������� ������ 23)���� ������ �������� �� ������� 24)���� ������ ��������� 25)��� ������ ������� 26)������ �����
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RemoveObject",
			(object[] p) => (object)entities.Remove((string)p[0])
		).Execute();

		//27)������ ���� ��������
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

		return newScope;
	}
}
