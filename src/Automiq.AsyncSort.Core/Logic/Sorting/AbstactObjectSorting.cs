using Automiq.AsyncSort.Core.Helpers;
using Automiq.AsyncSort.Core.Models;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

public static class AbstractObjectSorting
{
    /// <summary>
    /// Сортировка в соответствии с выбранными опциями
    /// </summary>
    public static void Sort(AbstractObject[] buffer, SortMethod sortMethod, CompareOpt compareOpt)
    {
        switch (sortMethod)
        {
            case SortMethod.Custom:
                CustomQuickSort(buffer, new AbstractObjectComparer(compareOpt));
                break;
            case SortMethod.DefaultDotnet:
                Array.Sort(buffer, new AbstractObjectComparer(compareOpt));
                break;
        }
    }
    
    /// <summary>
    /// Реализация сортировки вручную. Без острой надобности, но, допустим, для задания нужно
    /// </summary>
    static void CustomQuickSort(AbstractObject[] buffer, IComparer<AbstractObject> comparer) => BasicHelper.QuickSortRec(buffer, comparer, 0, buffer.Length - 1);
}