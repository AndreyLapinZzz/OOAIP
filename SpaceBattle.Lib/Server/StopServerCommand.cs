namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    public void execute(){
        int[] IdsArray = IoC.Resolve<BlockingCollection<int>>("ThreadsIDs").ToArray();
        foreach (int id in IdsArray) {
            ICommand softStopCommand = IoC.Resolve<ICommand>("Soft Stop The Thread", id);
            try{
                IoC.Resolve<ICommand>("Commands.SendCommand", id, softStopCommand).execute();
            } catch (Exception e)
            {
                IoC.Resolve<ICommand>("ExceptionHandler", e, softStopCommand).execute();
            }
        }
        IoC.Resolve<ICommand>("WaitForStopAllThread").execute();
    }
}
