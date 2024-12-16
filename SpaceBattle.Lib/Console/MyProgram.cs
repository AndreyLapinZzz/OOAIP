namespace SpaceBattle.Lib;
using System;
using System.Collections.Generic;
using System.Threading;
using Hwdtech;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
class MyProgram{
    static void Main(string[] args){
        int countThreads = int.Parse(args[0]);
        Console.WriteLine("Нажмите на любую клавишу для запуска");
        Console.ReadKey();
        IoC.Resolve<ICommand>("StartServerStrategy",countThreads).execute();
        Console.WriteLine("Сервер запущен");
        Console.WriteLine("Нажмите на любую клавишу для завершения работы сервера");
        Console.ReadKey();
        Console.WriteLine("Потоки останавливаются");
        IoC.Resolve<ICommand>("StopServerStrategy").execute();
        Console.WriteLine("Потоки остановеленны");
    }
}
