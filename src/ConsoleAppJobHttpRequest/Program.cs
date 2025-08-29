using Microsoft.Extensions.Configuration;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using System.Text.Json;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

logger.Information("Iniciando a execucao do Job...");

try
{
    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync(configuration["EndpointRequest"]);
    response.EnsureSuccessStatusCode();
    logger.Information("Notificacao enviada com sucesso!");
    logger.Information($"Dados recebidos = {JsonSerializer.Serialize(await response.Content.ReadAsStringAsync())}");
    logger.Information("Job executado com sucesso!");
}
catch (Exception ex)
{
    logger.Error($"Erro durante a execucao do Job: {ex.Message}");
    Environment.ExitCode = 1;
}