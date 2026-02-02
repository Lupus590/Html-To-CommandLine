using System.Text;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IHtmlToStringConverter
{
    public string ProcessFile(string filePath);
}

public class HtmlToStringConverter(ILogger<HtmlToStringConverter> logger) : IHtmlToStringConverter
{
	private readonly ILogger<HtmlToStringConverter> _logger = logger;

	public string ProcessFile(string filePath)
	{
		// TODO: look at line 3 for commemnt
		// <!--Created by Arma 3 Launcher: https://arma3.com-->
		// or maybe should look at the header metadata?
		// meta data sounds better

		var htmlDoc = new HtmlDocument();
		htmlDoc.Load(filePath);
		var modListTable = htmlDoc.DocumentNode
			.ChildNodes.First(n => n.Name.Equals("html", StringComparison.InvariantCultureIgnoreCase))
			.ChildNodes.First(n => n.Name.Equals("body", StringComparison.InvariantCultureIgnoreCase))
			.ChildNodes.Where(n => n.Name.Equals("div", StringComparison.InvariantCultureIgnoreCase))
				.First(n => n.GetClasses().Select(c => c.ToLowerInvariant()).Contains("mod-list")) // TODO: is there a better way to do this?
			.ChildNodes.First(n => n.Name.Equals("table", StringComparison.InvariantCultureIgnoreCase));

		var modListTableRows = modListTable
			.ChildNodes.Where(n => n.Name.Equals("tr", StringComparison.InvariantCultureIgnoreCase))
				.Where(n => n.Attributes.Select(a => a.Value.ToLowerInvariant()).Contains("ModContainer".ToLowerInvariant())); // TODO: is there a better way to do this?
		
		var stringBuilder = new StringBuilder();
		
		foreach (var modRow in modListTableRows)
		{
			var modName = modRow.ChildNodes.Where(n => n.Name.Equals("td", StringComparison.InvariantCultureIgnoreCase))
				.First(n => n.Attributes.Select(a => a.Value.ToLowerInvariant()).Contains("DisplayName".ToLowerInvariant())) // TODO: is there a better way to do this?
				.InnerText;
			stringBuilder.Append('@').Append(modName).Append(';');
		}

		return stringBuilder.ToString();
	}
}
