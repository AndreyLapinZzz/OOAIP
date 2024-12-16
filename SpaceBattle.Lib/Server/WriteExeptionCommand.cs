namespace SpaceBattle.Lib;
using Hwdtech;
using System.IO;
using System.Collections;

public class WriteExeptionCommand : ICommand
{
    public void execute()
    {
        IEnumerable<object> info = IoC.Resolve<IEnumerable<object>>("GetInfoExeption", ex);
        Using (TextWriter out = IoC.Resolve<TextWriter>("GetLogStream")){
            info.foreach (item => out.WriteLine(item) );
        }
    }
}
