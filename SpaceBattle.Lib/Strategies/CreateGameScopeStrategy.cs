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
		//1)аргс пустой
		object quantumTime = args[0];
		Queue<ICommand> queue = new Queue<ICommand>();
		Dictionary<string, IUObject> entities = new();
		//2)Нет стратегии 3)Ошибка в стратегии 4)Возвращает не тот тип
		string idGame = IoC.Resolve<string>("Server.GameID.New");

		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newScope).Execute();

		//5)Проверить что получено idGame 
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameID",
			(object[] p) => idGame
		).Execute();
		//6)Проверить что очередь пустая
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue",
			(object[] p) => queue
		).Execute();
		//7)Проверить что словарь пустой
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObjects",
			(object[] p) => entities
		).Execute();
		//8)Проверить что получено  quantumTime
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuantumTime",
			(object[] p) => quantumTime
		).Execute();
		//9)Список аргументов пустой 10)Если первый аргумент не команда 11)Есть лишние параметры 12)Всё ок, очередь пополнилась
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Enqueue",
			(object[] p) => {
				queue.Enqueue((SpaceBattle.Lib.ICommand)p[0]);
				return (object)true;
			}
		).Execute();
		//15)Есть лишние параметры 16)Нет команды в очереди 17)Очередь стала меньше
		
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Dequeue",
			(object[] p) => {
				return (object)queue.Dequeue();
			}
		).Execute();
		
		//18)Список аргументов пустой 19)Если первый аргумент не строчка 20)Есть лишние параметры 21)Всё ок
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetObject",
			(object[] p) => (object)entities[(string)p[0]]
		).Execute();
		//22)Список аргументов пустой 23)Если первый аргумент не строчка 24)Есть лишние параметры 25)Нет такого объекта 26)Объект удалён
		IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RemoveObject",
			(object[] p) => (object)entities.Remove((string)p[0])
		).Execute();

		//27)Старый скоп вернулся
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();

		return newScope;
	}
}
