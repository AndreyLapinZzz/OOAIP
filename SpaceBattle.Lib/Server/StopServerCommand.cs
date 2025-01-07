namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Concurrent;

public class StopServerCommand : ICommand
{
    public void execute(){
        //1)Нет такой стратегии в айоке 2)Возвращает не тот тип 3)Ошибка внутри стратегии
        int[] IdsArray = IoC.Resolve<BlockingCollection<int>>("ThreadsIDs").ToArray();
        foreach (int id in IdsArray) {
            //1)Нет такой стратегии в айоке 2)Возвращает не тот тип 3)Ошибка внутри стратегии
            ICommand softStopCommand = IoC.Resolve<ICommand>("Soft Stop The Thread", id);
            try{
                //Если вызывает ексепшен 2) Не вызывает
                softStopCommand.execute();
            } catch (Exception e)
            {
                //1)Нет такой стратегии в айоке 2)Возвращает не тот тип 3)Ошибка внутри стратегии 4) Внутри айкоманда ошибка
                IoC.Resolve<ICommand>("ExceptionHandler", e, softStopCommand).execute();
            }
        }
    }
}
