using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Lupus590.Arma3HtmlToCommandline.Domain.Tests;

public class HtmlToStringConverterTests
{
    [Fact]
    public void ProcessesFilesTest()
    {
        var testFilePath = "../../../testModlist.html";
        var serviceProvider = GetServiceProvider();
        
        var sut = serviceProvider.GetRequiredService<IHtmlToStringConverter>();
        var result = sut.ProcessFile(testFilePath);

        result.ShouldBe("@CBA_A3;@Speshal Core;@Animate - Rewrite;@ace;");
    }

    private static ServiceProvider GetServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IHtmlToStringConverter, HtmlToStringConverter>()
            .AddLogging();
        return serviceCollection.BuildServiceProvider();
    }
}
