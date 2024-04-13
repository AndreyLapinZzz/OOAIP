namespace SpaceBattle.Lib;
using Hwdtech;

public class SendCommandStrategy : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        MyThread thread = (MyThread)args[0];
        ICommand cmd = (ICommand)args[1];
        return new ActionCommand((arg) =>
        {
            ICommand send = IoC.Resolve<ICommand>("Thread.SendCommand", thread, cmd);
        });
    }
}
