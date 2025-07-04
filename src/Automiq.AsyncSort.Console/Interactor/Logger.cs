using Automiq.AsyncSort.Core.Utilities;

namespace Automiq.AsyncSort.Interactor;

public class Logger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}