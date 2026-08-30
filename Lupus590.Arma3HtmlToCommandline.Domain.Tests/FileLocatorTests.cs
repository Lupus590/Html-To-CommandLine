using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Lupus590.Arma3HtmlToCommandline.Domain.Tests;

public class FileLocatorTests
{
    [Fact]
    public void WhenFindArmaModlistHtmlFiles_ReturnsEnumerableOfFiles()
    {
        var testFilePath = "../../../testModlist.html";
        
        var serviceProvider = GetServiceProvider();
        var mockFileSystem = (MockFileSystem) serviceProvider.GetRequiredService<IFileSystem>();
        mockFileSystem.AddFile("testModlist.html", File.ReadAllText(testFilePath));
        
        var sut = serviceProvider.GetRequiredService<IFileLocator>();
        var result = sut.FindArmaModlistHtmlFiles().ToList();

		result.Count.ShouldBe(1);
        result.First().ShouldBe("testModlist.html");
    }

    private static ServiceProvider GetServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IFileLocator, FileLocator>()
            .AddTransient<IFileSystem, MockFileSystem>()
            .AddLogging();
        return serviceCollection.BuildServiceProvider();
    }
}
