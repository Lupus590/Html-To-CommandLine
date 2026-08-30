using Lupus590.Arma3HtmlToCommandline.Domain;
using Lupus590.Arma3HtmlToCommandline.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = Host.CreateApplicationBuilder();
host.Services.AddHostedService<Worker>()
    .AddTransient<IFileLocator, FileLocator>()
    .AddTransient<IModListHtmlToServerStringConverter,ModListHtmlToServerStringConverter>()
    .AddSerilog((services, loggerConfiguration) => loggerConfiguration.WriteTo.Console());

await host.Build().RunAsync();