using System.Collections;
using Automiq.AsyncSort.Core.Logic.Sorting;
using Automiq.AsyncSort.Core.Models;
using Automiq.AsyncSort.Core.Utilities;
using Xunit.Abstractions;

namespace Automiq.AsyncSort.Tests;

public class SortingTests 
{
    #region Поля-свойства
    private readonly ILogger _logger;
    #endregion

    #region Конструктор 
    public SortingTests(ITestOutputHelper testOutputHelper)
    {
        _logger = new TestLogger(testOutputHelper);
    }
    #endregion
    
    /// <summary>
    /// Тестирование успешного запуска и работы потоков генерации и сортировки
    /// </summary>
    [Fact]
    public async Task SortingThreadsWorkingWithNoExceptions()
    {
        Exception exception = null;
        try
        {
            var runner = new SortingRunner(_logger);
            var task = runner.Start(10, SortMethod.CustomCounting, CompareOpt.RGB);
            await Task.Delay(5_000);
            runner.Stop();
            await task;
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.Null(exception);
    }
    
    /// <summary>
    /// Проверка сортировки на тестовых данных из примера в задании
    /// </summary>
    [Theory]
    [ClassData(typeof(ExampleSortingTestData))]
    public void CheckSortingBasedOnBufferFromExample(AbstractObject[] buffer, SortMethod sortMethod, CompareOpt compareOpt, Color[] expectedColorOrder)
    {
        AbstractObjectSorting.Sort(buffer, sortMethod, compareOpt);

        bool result = true;
        for (int i = 0; i < buffer.Length; ++i)
        {
            var current = buffer.ElementAtOrDefault(i);
            var next = buffer.ElementAtOrDefault(i + 1);
            if (next is null) break;
            if (current.Color != next.Color && Array.IndexOf(expectedColorOrder, current.Color) > Array.IndexOf(expectedColorOrder, next.Color))
            {
                result = false;
                break;
            }
        }
        Assert.True(result);
    }
}

public class ExampleSortingTestData : IEnumerable<object[]>
{
    private static readonly AbstractObject[] _bufferFromExample = 
    [ 
        new (Color.Blue),
        new (Color.Blue),
        new (Color.Green),
        new (Color.Blue),
        new (Color.Red),
        new (Color.Green),
        new (Color.Green),
        new (Color.Green),
        new (Color.Red),
        new (Color.Red),
        new (Color.Blue),
        new (Color.Green),
        new (Color.Blue),
        new (Color.Blue),
        new (Color.Red),
        new (Color.Green),
    ];
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.GBR,  new [] { Color.Green, Color.Blue, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.GRB,  new [] { Color.Green, Color.Red, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.RGB,  new [] { Color.Red, Color.Green, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.RBG,  new [] { Color.Red, Color.Blue, Color.Green}];
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.BGR,  new [] { Color.Blue, Color.Green, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.CustomCounting, CompareOpt.BRG,  new [] { Color.Blue, Color.Red, Color.Green}];
        
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.GBR,  new [] { Color.Green, Color.Blue, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.GRB,  new [] { Color.Green, Color.Red, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.RGB,  new [] { Color.Red, Color.Green, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.RBG,  new [] { Color.Red, Color.Blue, Color.Green}];
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.BGR,  new [] { Color.Blue, Color.Green, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.CustomQuickSort, CompareOpt.BRG,  new [] { Color.Blue, Color.Red, Color.Green}];
        
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.GBR,  new [] { Color.Green, Color.Blue, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.GRB,  new [] { Color.Green, Color.Red, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.RGB,  new [] { Color.Red, Color.Green, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.RBG,  new [] { Color.Red, Color.Blue, Color.Green}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.BGR,  new [] { Color.Blue, Color.Green, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.BRG,  new [] { Color.Blue, Color.Red, Color.Green}];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}