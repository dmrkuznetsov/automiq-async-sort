using System.ComponentModel;

namespace Automiq.AsyncSort.Core.Logic.Sorting;
public enum SortMethod
{
    [Description("Оптимальный метод сортировки(мое итоговое решение)")]
    CustomSorting,
    [Description("Стандартный метод сортировки dotnet")]
    DefaultDotnet,
    [Description("Моя реализация Quicksort")]
    CustomQuickSort
}