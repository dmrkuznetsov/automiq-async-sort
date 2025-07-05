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
            var task = runner.Start(10, SortMethod.Custom, CompareOpt.RGB);
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
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.GBR,  new [] { Color.Green, Color.Blue, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.GRB,  new [] { Color.Green, Color.Red, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.RGB,  new Color[] { Color.Red, Color.Green, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.RBG,  new Color[] { Color.Red, Color.Blue, Color.Green}];
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.BGR,  new Color[] { Color.Blue, Color.Green, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.Custom, CompareOpt.BRG,  new Color[] { Color.Blue, Color.Red, Color.Green}];
        
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.GBR,  new [] { Color.Green, Color.Blue, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.GRB,  new [] { Color.Green, Color.Red, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.RGB,  new Color[] { Color.Red, Color.Green, Color.Blue}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.RBG,  new Color[] { Color.Red, Color.Blue, Color.Green}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.BGR,  new Color[] { Color.Blue, Color.Green, Color.Red}];
        yield return [ _bufferFromExample, SortMethod.DefaultDotnet, CompareOpt.BRG,  new Color[] { Color.Blue, Color.Red, Color.Green}];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}