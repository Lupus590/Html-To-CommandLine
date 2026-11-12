using System.Text;
using System.Text.RegularExpressions;
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

	private readonly Dictionary<string, string> _cdlcMapping = new()
	{
		{ "S.O.G. Prairie Fire", "vn" },
		{ "Western Sahara", "ws" },
		{ "Spearhead 1944", "spe" },
		{ "Global Mobilization", "gm" },
		{ "CSLA Iron Curtain", "csla" },
		{ "Reaction Forces", "rf" },
		{ "Expeditionary Forces", "EF" },
	};

	
	private readonly Regex _clientModIdRegex = new(@"^.*\?id=([0-9]*)$", RegexOptions.None, TimeSpan.FromSeconds(1));

	private static string GetDisplayName(HtmlNode tableRow)
	{
		var displayName = tableRow.SelectNodes("td")?.FirstOrDefault(node => node.GetAttributeValue("data-type", string.Empty) == "DisplayName");

		return displayName?.InnerText ?? string.Empty;
	}

	private StringBuilder ConvertDlc(StringBuilder stringBuilder, HtmlNode? dlcListTable)
	{
		_logger.LogDebug("Found CDLC list table, extracting rows");

		var rows = dlcListTable?.SelectNodes("tr")?.AsEnumerable() ?? Enumerable.Empty<HtmlNode>();
		foreach (var row in rows)
		{
			var displayName = GetDisplayName(row);
			var mappedName = _cdlcMapping[displayName];
			stringBuilder.Append(mappedName).Append(';');
			_logger.LogDebug("Extracted CDLC `{cdlcName}` as `{mappedName}`", displayName, mappedName);
		}

		return stringBuilder;
	}

	private bool IsClientSide(HtmlNode tableRow)
	{
		var clientSideIds = new List<string>()
		{
			"3804661268",
		};

		var url = string.Empty;
		var tableDatas = tableRow?.SelectNodes("td")?.AsEnumerable() ?? Enumerable.Empty<HtmlNode>();
		foreach (var tableData in tableDatas)
		{
			var anchor = tableData.SelectNodes("a")?.FirstOrDefault();
			if ( anchor is not null)
			{
				url = anchor.InnerText;
				break;
			}
		}

		var id = _clientModIdRegex.Match(url).Groups[1].Value;

		return clientSideIds.Contains(id);
	}

	private StringBuilder ConvertMods(StringBuilder stringBuilder, HtmlNode? modListTable)
	{
		_logger.LogDebug("Found modlist table, extracting rows");

		var rows = modListTable?.SelectNodes("tr")?.AsEnumerable() ?? Enumerable.Empty<HtmlNode>();
		foreach (var row in rows)
		{
			var displayName = GetDisplayName(row);

			if (IsClientSide(row))
			{
				_logger.LogDebug("Found clientside mod `{modName}`, skipping from commandline", displayName);
				continue;
			}

			stringBuilder.Append('@').Append(displayName).Append(';');
			_logger.LogDebug("Extracted mod `{modName}`", displayName);
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
