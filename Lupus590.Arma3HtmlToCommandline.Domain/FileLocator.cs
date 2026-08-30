using System.IO.Abstractions;
using HtmlAgilityPack;

namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IFileLocator
{
    IEnumerable<string> FindArmaModlistHtmlFiles();
}

public class FileLocator : IFileLocator
{
	private readonly IFileSystem fileSystem;

	public FileLocator(IFileSystem fileSystem)
	{
		this.fileSystem = fileSystem;
	}

	private bool IsArmaModlist(string filePath)
	{
		var extention = fileSystem.Path.GetExtension(filePath);
		if (!extention.Equals("html", StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}

		var htmlText = fileSystem.File.ReadAllText(filePath);
		var htmlDocument = new HtmlDocument();
		htmlDocument.LoadHtml(htmlText);		
		
		var generator = htmlDocument?.DocumentNode.SelectNodes("meta")
			?.FirstOrDefault(t => t.Attributes["name"]?.Value == "generator");

		return generator?.Attributes["content"]?.Value == "Arma 3 Launcher - https://arma3.com";
	}

	public IEnumerable<string> FindArmaModlistHtmlFiles()
	{
		var workingFolder = fileSystem.Directory.GetCurrentDirectory();

		var armaModlists = fileSystem.Directory.EnumerateFiles(workingFolder)
			.Where(IsArmaModlist);

		return armaModlists;
	}
}
