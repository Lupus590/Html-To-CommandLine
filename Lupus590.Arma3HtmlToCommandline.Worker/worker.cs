using Lupus590.Arma3HtmlToCommandline.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lupus590.Arma3HtmlToCommandline.Worker;

public class Worker(IHtmlToStringConverter htemlToStringConverter, ILogger<Worker> logger, IFileLocator fileLocator) : BackgroundService
{
	private readonly IHtmlToStringConverter _htemlToStringConverter = htemlToStringConverter;
	private readonly ILogger<Worker> _logger = logger;
	private readonly IFileLocator _fileLocator = fileLocator;

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var fileToProcess = _fileLocator.FindHtmlFile();
		var commandLine = _htemlToStringConverter.ProcessFile(fileToProcess);

		return Task.CompletedTask;

	}
}
