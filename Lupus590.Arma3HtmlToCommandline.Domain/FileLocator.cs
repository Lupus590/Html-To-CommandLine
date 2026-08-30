using System.IO.Abstractions;
using HtmlAgilityPack;

namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IFileLocator
{
    IEnumerable<string> FindArmaModlistHtmlFiles();
}

public class FileLocator(IFileSystem fileSystem) : IFileLocator
{
	private readonly IFileSystem _fileSystem = fileSystem;

	private bool IsArmaModlist(string filePath)
	{
		var extention = _fileSystem.Path.GetExtension(filePath);
		if (!extention.Equals("html", StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}

		var htmlText = _fileSystem.File.ReadAllText(filePath);
		var htmlDocument = new HtmlDocument();
		htmlDocument.LoadHtml(htmlText);		
		
		var generator = htmlDocument?.DocumentNode.SelectNodes("meta")
			?.FirstOrDefault(t => t.Attributes["name"]?.Value == "generator");

		return generator?.Attributes["content"]?.Value == "Arma 3 Launcher - https://arma3.com";
	}

	public IEnumerable<string> FindArmaModlistHtmlFiles()
	{
		var workingFolder = _fileSystem.Directory.GetCurrentDirectory();

		var armaModlists = _fileSystem.Directory.EnumerateFiles(workingFolder)
			.Where(IsArmaModlist);

		return armaModlists;
	}
}
