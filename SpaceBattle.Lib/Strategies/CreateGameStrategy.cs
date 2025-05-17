using Hwdtech;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;

namespace SpaceBattle.Lib;

public class CreateGameStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        object newGameScope = IoC.Resolve<object>("CreateGameScopeStrategy", args[0]);
        object oldScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newGameScope).Execute();
        string gameId = IoC.Resolve<string>("GetGameID");
        Queue<ICommand> queue = IoC.Resolve<Queue<ICommand>>("GetQueue");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", oldScope).Execute();
        ICommand gameCommand = IoC.Resolve<ICommand>("GameCommand.Create", gameId, newGameScope, queue);
        return gameCommand;
    }
}
