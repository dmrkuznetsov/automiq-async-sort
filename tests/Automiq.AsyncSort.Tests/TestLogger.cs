using Automiq.AsyncSort.Core.Utilities;
using Xunit.Abstractions;

namespace Automiq.AsyncSort.Tests;

public class TestLogger : ILogger 
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestLogger(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    public void Log(string message) => _testOutputHelper.WriteLine(message);
}