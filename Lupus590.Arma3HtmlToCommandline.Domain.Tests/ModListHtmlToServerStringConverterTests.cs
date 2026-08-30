using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Lupus590.Arma3HtmlToCommandline.Domain.Tests;

public class ModListHtmlToServerStringConverterTests
{
    [Fact]
    public void WhenConvertWithValidHTML_ReturnsCorrectString()
    {
        var testFilePath = "../../../testModlist.html";
        var testDoc = new HtmlDocument();
		testDoc.LoadHtml(testFilePath);
        
        var serviceProvider = GetServiceProvider();
        
        var sut = serviceProvider.GetRequiredService<IModListHtmlToServerStringConverter>();
        var result = sut.Convert(testDoc);

        result.ShouldBe("@CBA_A3;@Speshal Core;@Animate - Rewrite;@ace;");
    }

    private static ServiceProvider GetServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IModListHtmlToServerStringConverter, ModListHtmlToServerStringConverter>()
            .AddLogging();
        return serviceCollection.BuildServiceProvider();
    }
}
