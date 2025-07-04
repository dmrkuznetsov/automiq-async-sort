using Automiq.AsyncSort.Interactor;

namespace Automiq.AsyncSort;

class Program
{
    /// <summary>
    /// В отсутствие подробных треборваний к интерфейсу, реализуем взаимодействие с пользователем через консольньный диалог
    /// </summary>
    static void Main(string[] args)
    {
        Console.WriteLine("Реализация тестового задания по асинхронной сортировке объектов");
        
        //Зависимости без DI (задача небольшая)
        var logger = new Logger();
        var interactor = new ConsoleInteractor(logger);
        interactor.Start().Wait();
    }
}