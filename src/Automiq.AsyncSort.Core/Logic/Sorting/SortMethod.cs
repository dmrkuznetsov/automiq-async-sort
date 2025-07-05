using System.ComponentModel;

namespace Automiq.AsyncSort.Core.Logic.Sorting;
public enum SortMethod
{
    [Description("Стандартный метод сортировки dotnet")]
    DefaultDotnet,
    [Description("Моя реализация Quicksort")]
    CustomQuickSort,
    [Description("Моя реализация сортировки подсчетом")]
    CustomCounting
}