using System.Text;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IModListHtmlToServerStringConverter
{
    public string Convert(HtmlDocument htmlDoc);
}

public class ModListHtmlToServerStringConverter(ILogger<ModListHtmlToServerStringConverter> logger) : IModListHtmlToServerStringConverter
{
	private readonly ILogger<ModListHtmlToServerStringConverter> _logger = logger;

	private static string GetDisplayName(HtmlNode tableRow)
	{
		var displayName = tableRow.SelectNodes("td")?.FirstOrDefault(node => node.GetAttributeValue("data-type", string.Empty) == "DisplayName");

		return displayName?.InnerText ?? string.Empty;
	}

	private StringBuilder ConvertDlc(StringBuilder stringBuilder, HtmlNode? dlcListTable)
	{
		_logger.LogDebug("Found CDLC list table, extracting rows");

		return stringBuilder;
	}

	private StringBuilder ConvertMods(StringBuilder stringBuilder, HtmlNode? modListTable)
	{
		_logger.LogDebug("Found modlist table, extracting rows");

		var rows = modListTable?.SelectNodes("tr")?.AsEnumerable() ?? Enumerable.Empty<HtmlNode>();
		foreach (var row in rows)
		{
			var displayName = GetDisplayName(row);
			stringBuilder.Append('@').Append(displayName).Append(';');
			_logger.LogDebug("Extracted mod {modName}", displayName);
		}
		
		return stringBuilder;
	}

	public string Convert(HtmlDocument htmlDocument)
	{
		_logger.LogDebug("Looking for mod list table");

		var html = htmlDocument?.DocumentNode.SelectNodes("html")?.FirstOrDefault();
		var body = html?.SelectNodes("body")?.FirstOrDefault();
		var divs = body?.SelectNodes("div");
		var stringBuilder = new StringBuilder();

		var dlcListDiv = divs?.FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "dlc-list");
		var dlcListTable = dlcListDiv?.SelectNodes("table")?.FirstOrDefault();
		stringBuilder = ConvertDlc(stringBuilder, dlcListTable);

		var modListDiv = divs?.FirstOrDefault(node => node.GetAttributeValue("class", string.Empty) == "mod-list");
		var modListTable = modListDiv?.SelectNodes("table")?.FirstOrDefault();
		stringBuilder = ConvertMods(stringBuilder, modListTable);

		_logger.LogInformation("Finished");
		
		return stringBuilder.ToString();
	}
}
