using Lupus590.Arma3HtmlToCommandline.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lupus590.Arma3HtmlToCommandline.Worker;

public class Worker(IModListHtmlToServerStringConverter modListHtmlToServerStringConverter, ILogger<Worker> logger, IFileLocator fileLocator) : BackgroundService
{
	private readonly IModListHtmlToServerStringConverter _modListHtmlToServerStringConverter = modListHtmlToServerStringConverter;
	private readonly ILogger<Worker> _logger = logger;
	private readonly IFileLocator _fileLocator = fileLocator;

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		//var fileToProcess = _fileLocator.FindArmaModlistHtmlFiles();
		//var commandLine = _modListHtmlToServerStringConverter.Convert(fileToProcess);

		return Task.CompletedTask;

	}
}
