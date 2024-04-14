namespace SpaceBattle.Lib;
using Hwdtech;

public class SoftStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        string thread_id = (string)args[0];
        Action? acmd = null;
        if (args.Length > 1)
        {
            acmd = (Action)args[1];
        }
        return new ActionCommand((arg) =>
        {
            MyThread th = IoC.Resolve<MyThread>("GetThread", thread_id);
            var cmd = new SoftStopCommand(th);
            IoC.Resolve<ActionCommand>("Thread.SendCommand", thread_id, cmd).execute();
            if (acmd != null)
            {
                acmd();
            }
        });
    }
}
