using Automiq.AsyncSort.Core.Helpers;
using Automiq.AsyncSort.Core.Logic.Sorting;
using Automiq.AsyncSort.Core.Utilities;

namespace Automiq.AsyncSort.Interactor;

/// <summary>
/// Класс для работы с логикой сортировки через консоль
/// </summary>
public class ConsoleInteractor 
{
    #region Поля-свойства 
    private ILogger _logger;
    /// <summary>
    /// Текущее состояние консольного диалога
    /// </summary>
    private DialogState _currDialogState = DialogState.BufferSizeSelection;
    /// <summary>
    /// Выбранный размер буфера генерируемых объектов
    /// </summary>
    private int _selectedBufferSize = 10;
    /// <summary>
    /// Выбранный метод сравнения цветов
    /// </summary>
    private CompareOpt _selectedCompareOpt = CompareOpt.RGB;
    /// <summary>
    /// Выбранный метод сортировки
    /// </summary>
    private SortMethod _selectedSortMethod = SortMethod.DefaultDotnet;
    /// <summary>
    /// Раннер, где крутится генерация и сортировка данных
    /// </summary>
    private SortingRunner _sortingRunner;
    /// <summary>
    /// Максимальный размер буфера
    /// </summary>
    private const int MaxBufferSize = 100_00;
    /// <summary>
    /// Минимальный размер буфера
    /// </summary>
    private const int MinBufferSize = 2;
    #endregion

    #region Конструктор
    public ConsoleInteractor(ILogger logger)
    {
        _logger = logger;
    }
    #endregion

    #region Методы
    /// <summary>
    /// Запуск
    /// </summary>
    public async Task Start()
    {
        Console.CancelKeyPress +=  (_, e) =>
        {
            if (_sortingRunner is not null)
            {
                _sortingRunner.Stop();
                e.Cancel = true;
            }
        };
        await InvokeCurrentState();
    }

    /// <summary>
    /// Рекурсивный метод для потенциального возвращения на предыдущие шаги
    /// </summary>
    private async Task InvokeCurrentState()
    {
        switch (_currDialogState)
        {
            case DialogState.BufferSizeSelection: SelectObjBufferSize(); break;
            case DialogState.CompareMethodSelection: SelectCompareMethod(); break;
            case DialogState.SortingMethodSelection: SelectSortMethod(); break;
            case DialogState.RunnerExecution: await RunSorting(); break;
        }
        //Зацикливаем работу с консолью, чтобы вовращаться на первый шаг
        await InvokeCurrentState(); 
    }
    
    /// <summary>
    /// Выбор размера буфера генерируемых объектов
    /// </summary>
    private void SelectObjBufferSize()
    {
        Console.WriteLine("Выберите размер буфера генерируемых объектов");
        var res = Console.ReadLine();
        if (!int.TryParse(res, out var intResult) || intResult < MinBufferSize || intResult > MaxBufferSize)
        {
            Console.WriteLine($"Неверный формат размера буфера: int, min : {MinBufferSize}, max : {MaxBufferSize}");
            return;
        }
        _selectedBufferSize = intResult;
        _currDialogState = DialogState.CompareMethodSelection;
    }
    
    /// <summary>
    /// Выбор способа сравнения при сортировке
    /// </summary>
    private void SelectCompareMethod()
    {
        var res = (CompareOpt?)SelectEnumOption<CompareOpt>("Выберите способ сравнения цветов для сортировки");
        if (res.HasValue)
        {
            _selectedCompareOpt = res.Value;
            _currDialogState = DialogState.SortingMethodSelection;
        }
    }
    
    /// <summary>
    /// Выбор метода сортировки
    /// </summary>
    private void SelectSortMethod()
    {
        var res = (SortMethod?)SelectEnumOption<SortMethod>("Выберите метод сортировки");
        if (res.HasValue)
        {
            _selectedSortMethod = res.Value;
            _currDialogState = DialogState.RunnerExecution;
        }
    }

    /// <summary>
    /// Выбор опции перечисления через консоль
    /// </summary>
    private Enum SelectEnumOption<T>(string message) where T : Enum
    {
        var enumOpts = Enum.GetValues(typeof(T))
            .Cast<Enum>().ToArray();
        
        var opts = enumOpts.Select(x => x.GetDescriptionAttribute()).ToArray();
                
        Console.WriteLine($"{message}:\n{opts.Select(x=>$"{Array.IndexOf(opts, x) + 1}) {x}").Aggregate((x,y)=>$"{x} \n{y}" )}");
        var optIndexStr = Console.ReadLine();
                
        if (!int.TryParse(optIndexStr, out var optIndex))
        {
            Console.WriteLine($"Некорректный ввод: {optIndexStr}");
            return null;
        }
                
        var selectedOpt = enumOpts.ElementAtOrDefault(optIndex - 1);
        if (selectedOpt is null)
        {
            Console.WriteLine($"Выбранная опция: {optIndexStr} - не найдена");
            return null;
        }
        return selectedOpt;
    }
    
    /// <summary>
    /// Запуск сортировки
    /// </summary>
    private async Task RunSorting()
    {
        _sortingRunner = new SortingRunner(_logger);
        await _sortingRunner.Start(_selectedBufferSize, _selectedSortMethod, _selectedCompareOpt);
        _currDialogState = DialogState.BufferSizeSelection;
        _sortingRunner = null;
    }
    #endregion
}