namespace SpaceBattle.Lib;
using Hwdtech;
using System.IO;
using System.Collections;

public class WriteExeptionCommand : ICommand
{
    private Exception ex;
    public WriteExeptionCommand(Exception ex)
    {
        this.ex = ex;
    }
    public void execute()
    {
        IEnumerable<object> info = IoC.Resolve<IEnumerable<object>>("GetInfoExeption", ex);
        using (TextWriter myOut = IoC.Resolve<TextWriter>("GetLogStream")){
            info.ToList().ForEach (item => myOut.WriteLine(item) );
        }
    }
}
