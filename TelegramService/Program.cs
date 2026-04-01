using services.Handlers;
using servicios.Handlers;

SqliteHandler.ConnectionString = "Data source=Database/LoginDataBase.db";

string token = "8636517614:AAGtMwdea26SlSefW9nx9WJIajIIUtKnWqQ";

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
