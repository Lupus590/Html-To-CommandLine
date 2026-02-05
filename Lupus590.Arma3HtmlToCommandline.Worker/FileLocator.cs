namespace Lupus590.Arma3HtmlToCommandline.Worker;

public interface IFileLocator
{
    string FindHtmlFile();
}

public class FileLocator : IFileLocator
{
	public string FindHtmlFile()
	{
		// filter by html
		// read the header part of the html to make sure it's an Arma preset/list
		throw new NotImplementedException();
	}
}
