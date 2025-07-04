namespace Automiq.AsyncSort.Core.Models;

/// <summary>
/// Некий объект, по условию задачи характеризующийся цветом
/// </summary>
public class AbstractObject
{
    public Color Color { get; }

    public AbstractObject(Color color)
    {
        Color = color;
    }
}