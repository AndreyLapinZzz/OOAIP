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
        //1)Нет такой стратегии в айоке 2)Возвращает не тот тип 3)Ошибка внутри стратегии
        IEnumerable<object> info = IoC.Resolve<IEnumerable<object>>("GetInfoExeption", ex);
        //1)Нет такой стратегии в айоке 2)Возвращает не тот тип 3)Ошибка внутри стратегии
        using (TextWriter myOut = IoC.Resolve<TextWriter>("GetLogStream")){
            //1)Есть ошибка внутри врайтлайн 2)Всё ок
            info.ToList().ForEach (item => myOut.WriteLine(item) );
        }
    }
}
