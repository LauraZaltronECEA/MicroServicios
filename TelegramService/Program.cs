using services.Handlers;
using servicios.Handlers;
using System;
using TelegramService;

EnvLoader.LoadEnv(); // carga variables desde TelegramService/.env

SqliteHandler.ConnectionString = Environment.GetEnvironmentVariable("SQLITE_CONNECTION")
    ?? "Data source=Database/LoginDataBase.db";

string token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN")
    ?? throw new InvalidOperationException("TELEGRAM_TOKEN no está configurada.");

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await TelegramHandler.RunBotAsync(token.Trim(), cts.Token);

try
{
    await Task.Delay(Timeout.Infinite, cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Bot detenido por el usuario.");
}
