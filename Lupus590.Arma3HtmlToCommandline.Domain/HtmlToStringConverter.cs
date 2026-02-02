namespace Lupus590.Arma3HtmlToCommandline.Domain;

public interface IHtmlToStringConverter
{
    public string ProcessFile(string filePath);
}

public class HtmlToStringConverter : IHtmlToStringConverter
{
	public string ProcessFile(string filePath)
	{
		throw new NotImplementedException();
	}
}
