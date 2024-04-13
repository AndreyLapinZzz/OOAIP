namespace SpaceBattle.Lib;
using Hwdtech;

public class SoftStopThreadStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread thread = (MyThread)args[0];
        Action? acmd = null;
        if (args.Length > 1)
        {
            acmd = (Action)args[1];
        }
        return new ActionCommand((arg) =>
        {
            var cmd = new SoftStopCommand(thread);
            IoC.Resolve<ActionCommand>("Thread.SendCommand", thread, cmd).execute();
            if (acmd != null)
            {
                acmd();
            }
        });
    }
}
