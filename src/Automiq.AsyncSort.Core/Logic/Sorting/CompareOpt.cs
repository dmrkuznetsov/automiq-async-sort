using System.ComponentModel;

namespace Automiq.AsyncSort.Core.Logic.Sorting;

/// <summary>
/// Опции сравнения абстрактного объекта по цвету
/// </summary>
public enum CompareOpt
{
    [Description("Красный > Зеленый > Синий")]
    RGB,
    [Description("Красный > Синий > Зеленый")]
    RBG,
    [Description("Зеленый > Красный > Синий")]
    GRB,
    [Description("Зеленый > Синий > Красный")]
    GBR,
    [Description("Синий > Красный > Зеленый")]
    BRG,
    [Description("Синий > Зеленый > Красный")]
    BGR
}