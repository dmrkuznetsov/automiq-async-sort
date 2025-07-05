using Automiq.AsyncSort.Core.Models;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

/// <summary>
/// Класс, реализующий сравнение абстрактных объектов
/// </summary>
public class AbstractObjectComparer : IComparer<AbstractObject>
{
    private readonly CompareOpt _compareOpt;

    private readonly ColorComparer _colorComparer;
    public AbstractObjectComparer(CompareOpt compareOpt)
    {
        _compareOpt = compareOpt;
        _colorComparer = new ColorComparer(compareOpt);
    }
    public int Compare(AbstractObject? x, AbstractObject? y)
    {
        if (x is null || y is null) return 0;
        return _colorComparer.Compare(x.Color, y.Color);
    }
}

public class ColorComparer : IComparer<Color>
{
    private readonly CompareOpt _compareOpt;
    public ColorComparer(CompareOpt compareOpt)
    {
        _compareOpt = compareOpt;
    }
    
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
    public int Compare(Color x, Color y)
    {
        if (!_comparisonWeights.TryGetValue(_compareOpt, out var weights)
            || !weights.TryGetValue(x, out var weightX)
            || !weights.TryGetValue(y, out var weightY)) return 0;
        return weightX.CompareTo(weightY);
    }
}