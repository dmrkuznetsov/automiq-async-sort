using System.ComponentModel;

namespace Automiq.AsyncSort.Interactor;

public enum GenerationMethod
{
    [Description("Случайный набор")]
    Random,
    [Description("Ручной ввод")]
    Manual
}