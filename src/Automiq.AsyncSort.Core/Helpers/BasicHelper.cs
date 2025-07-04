namespace Automiq.AsyncSort.Core.Helpers;

public static class BasicHelper 
{
    /// <summary>
    /// Реализация алгоритма QuickSort
    /// </summary>
    public static T[] QuickSortRec<T>(T[] array, IComparer<T> comparer, int leftIndex , int rightIndex)
    {
        if(array.Length <= 1)
            return array;
        
        var i = leftIndex;
        var j = rightIndex;
        
        var pivot = array[leftIndex];
        
        while (i <= j)
        {
            while (comparer.Compare(array[i], pivot) < 0)
            {
                i++;
            }
        
            while (comparer.Compare(array[j], pivot) > 0)
            {
                j--;
            }
            
            if (i <= j)
            {
                (array[i], array[j]) = (array[j], array[i]);
                i++;
                j--;
            }
        }
    
        if (leftIndex < j)
            QuickSortRec(array, comparer, leftIndex, j);
        if (i < rightIndex)
            QuickSortRec(array, comparer, i, rightIndex);
        return array;
    }
}