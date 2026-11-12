using System;
using System.Collections.Frozen;

namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IConfig
{
    IReadOnlyList<string> ClientSideIds {get;}
    FrozenDictionary<string, string> CdlcMapping {get;}
}

public class Config : IConfig
{
    static readonly Dictionary<string, string> _cdlcMapping = new()
	{
		{ "S.O.G. Prairie Fire", "vn" },
		{ "Western Sahara", "ws" },
		{ "Spearhead 1944", "spe" },
		{ "Global Mobilization", "gm" },
		{ "CSLA Iron Curtain", "csla" },
		{ "Reaction Forces", "rf" },
		{ "Expeditionary Forces", "EF" },
	};


	static readonly List<string> _clientSideIds = new()
	{
		"3804661268",
	};

	public IReadOnlyList<string> ClientSideIds => _clientSideIds;

	public FrozenDictionary<string, string> CdlcMapping => _cdlcMapping.ToFrozenDictionary();
}
