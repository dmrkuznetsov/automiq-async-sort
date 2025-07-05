using Automiq.AsyncSort.Core.Helpers;
using Automiq.AsyncSort.Core.Models;
using Automiq.AsyncSort.Core.Utilities;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

public class SortingRunner
{
    #region Поля-свойства
    /// <summary>
    /// Логгер
    /// </summary>
    private ILogger _logger;
    /// <summary>
    /// Таска для генерации объектов
    /// </summary>
    private Task _genTask;
    /// <summary>
    /// Таска для сортировки объектов
    /// </summary>
    private Task _sortTask;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    /// <summary>
    /// Очередь для обработки сгенерированных объектов
    /// </summary>
    private Queue<AbstractObject[]> _bufferQueue = new Queue<AbstractObject[]>();
    /// <summary>
    /// Лок-объект для синхронизации потоков
    /// </summary>
    private readonly object _lock = new object();
    #endregion

    #region Конструктор 
    public SortingRunner(ILogger logger)
    {
        _logger = logger;
    }
    #endregion

    #region Методы 
    /// <summary>
    /// Запуск двух тасок с генерацией и сортировкой
    /// </summary>
    public async Task Start(int objNumber, SortMethod sortMethod, CompareOpt compareOpt)
    {
        var token = _cancellationTokenSource.Token;
        _genTask = new Task(() =>
        {
            try
            {
                while (true)
                {
                    //Выход из потока генерации
                    if (token.IsCancellationRequested) break;
                
                    //Эмуляция выполнения внешних вызовов
                    Thread.Sleep(1000);

                    //Генерация случайных объектов
                    var buffer = GenerateRandomBuffer(objNumber);
                    lock (_lock)
                    {
                        _bufferQueue.Enqueue(buffer);
                    }
                    _logger.Log($"Сгенерированы объекты: {buffer.Select(x=>x.Color.ToString()).Aggregate((x,y)=>$"{x}, {y}")}");
                }
            }
            catch(Exception ex)
            {
                _logger.Log($"Ошибка в генерирующем потоке: {ex.Message}. Остановка исполнения"); 
                Stop();
            }
        }, token);

        _sortTask = new Task(() =>
        {
            try
            {
                while (true)
                {
                    //Выход из потока сортировки 
                    if (token.IsCancellationRequested) break;
                
                    //Считаем для этой задачи, что непосредственно объекты и буфер не меняются в генерирующем потоке
                    AbstractObject[] buffer = null;
                    lock (_lock)
                    {
                        if (_bufferQueue.TryDequeue(out var result)) buffer = result;
                    }

                    if (buffer is not null)
                    {
                        AbstractObjectSorting.Sort(buffer, sortMethod, compareOpt);
                        _logger.Log($"Результат сортировки объектов: {buffer.Select(x=>x.Color.ToString()).Aggregate((x,y)=>$"{x}, {y}")}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Ошибка в сортирующем потоке: {ex.Message}. Остановка исполнения");
                Stop();
            }
        }, token);
        _genTask.Start();
        _sortTask.Start();
        await Task.WhenAll(_genTask, _sortTask);
    }
    
    /// <summary>
    /// Генерация случайного буфера
    /// </summary>
    public AbstractObject[] GenerateRandomBuffer(int objNumber)
    {
        Random random = new Random();
        var colors = Enum.GetValues(typeof(Color));
        var buffer = new AbstractObject[objNumber];
        for (int i = 0; i < objNumber; i++)
        {
            var randomColor = (Color)colors.GetValue(random.Next(colors.Length));
            buffer[i] = new AbstractObject(randomColor);
        }
        return buffer;
    }
    
    /// <summary>
    /// Остановка потоков
    /// </summary>
    public void Stop()
    {
        if (_cancellationTokenSource.IsCancellationRequested) return;
        _cancellationTokenSource.Cancel();
    }
    #endregion
}