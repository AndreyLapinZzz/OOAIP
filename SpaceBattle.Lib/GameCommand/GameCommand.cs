using Hwdtech;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    IReceiver queue;
    object scope;
    string gameId;
    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();

    public GameCommand(string gameId, object scope, IReceiver queue)
    {
        this.gameId = gameId;
        this.scope = scope;
        this.queue = queue;
    }

    public void execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",scope).Execute();
        time.Reset();
        while(time.ElapsedMilliseconds < IoC.Resolve<int>("GetGameTimeLimit")){
            time.Start();
            var cmd = queue.Receive();
            try 
            {
                cmd.execute();
            }
            catch
            {
                IoC.Resolve<ICommand>("Game.ExceptionHandler", new Exception(), cmd.GetType());
            }
            time.Stop();
        }
    }
}
