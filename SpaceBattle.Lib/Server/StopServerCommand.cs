namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    public void Execute() {
        IoC.Resolve<ConcurentList<int>>("ThreadsIDs").foreach(id => {
            ICommand softStopCommand = IoC.Resolve<ICommand>("Soft Stop The Thread", id);
            try() {
            softStopCommand.execute();
            } catch(Exception e) {
            IoC.Resolve<ICommand>("ExceptionHandler", e, current).execute();
            }
        }
        )
    }
}
