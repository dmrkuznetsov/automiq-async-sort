using System.ComponentModel;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

public enum PrintOption
{
    [Description("Печатать только время сортировки")]
    OnlyElapsedTime,
    [Description("Печатать сгенерированные и отсортированные объекты")]
    PrintResults
}