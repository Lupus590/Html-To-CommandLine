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
		if (!extention.Equals(".html", StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}

		var htmlText = _fileSystem.File.ReadAllText(filePath);
		var htmlDocument = new HtmlDocument();
		htmlDocument.LoadHtml(htmlText);
		
		var html = htmlDocument?.DocumentNode.SelectNodes("html")?.FirstOrDefault();
		var head = html?.SelectNodes("head")?.FirstOrDefault();
		var meta = head?.SelectNodes("meta");
		var generator = meta?.FirstOrDefault(node => node.GetAttributeValue("name", string.Empty) == "generator");
		var result = generator?.GetAttributeValue("content", string.Empty) == "Arma 3 Launcher - https://arma3.com";

		return result;
	}

	public IEnumerable<string> FindArmaModlistHtmlFiles()
	{
		var workingFolder = _fileSystem.Directory.GetCurrentDirectory();

		var armaModlists = _fileSystem.Directory.EnumerateFiles(workingFolder)
			.Where(IsArmaModlist);

		return armaModlists;
	}
}
