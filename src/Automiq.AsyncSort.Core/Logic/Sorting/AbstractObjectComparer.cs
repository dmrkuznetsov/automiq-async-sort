using Automiq.AsyncSort.Core.Models;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

/// <summary>
/// Класс, реализующий сравнение абстрактных объектов
/// </summary>
public class AbstractObjectComparer(CompareOpt opt) : IComparer<AbstractObject>
{
    private static readonly Dictionary<CompareOpt, Dictionary<Color, int>> _comparisonWeights =
        new ()  
        {
            { CompareOpt.RGB, new () { { Color.Red, 0 }, { Color.Green, 1 }, { Color.Blue, 2 }}},
            { CompareOpt.RBG, new() { { Color.Red, 0 }, { Color.Blue, 1 }, { Color.Green, 2 }}},
            { CompareOpt.GRB, new() { { Color.Green, 0 }, { Color.Red, 1 }, { Color.Blue, 2 }}},
            { CompareOpt.GBR, new() { { Color.Green, 0 }, { Color.Blue, 1 }, { Color.Red, 2 }}},
            { CompareOpt.BRG, new() { { Color.Blue, 0 }, { Color.Red, 1 }, { Color.Green, 2 }}},
            { CompareOpt.BGR, new() { { Color.Blue, 0 }, { Color.Green, 1 }, { Color.Red, 2 }}}
        };
    
    public int Compare(AbstractObject? x, AbstractObject? y)
    {
        if (x is null || y is null) return 0;
        if (!_comparisonWeights.TryGetValue(opt, out var weights)
            || !weights.TryGetValue(x.Color, out var weightX)
            || !weights.TryGetValue(y.Color, out var weightY)) return 0;
        return weightX.CompareTo(weightY);
    }
}