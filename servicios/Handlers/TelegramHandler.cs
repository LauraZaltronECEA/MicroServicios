
using servicios.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace services.Handlers
{
    public class TelegramHandler
    {
        // token del bot

        public static async Task RunBotAsync(string token, CancellationToken cancellationToken)
        {
            var botClient = new TelegramBotClient(token);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message]//permitimos intercambiode mensajes
            };

            botClient.StartReceiving(//recibir mensajes
                updateHandler: (client, update, ct) => OnUpdateAsync(client, update, ct),
                pollingErrorHandler: OnPollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken = cancellationToken
            );

            var me = await botClient.GetMeAsync(cancellationToken);

            var name = string.IsNullOrEmpty(me.Username) ? me.FirstName : $"@{me.Username}";
            Console.WriteLine($"Bot {name} esperando mensajes... (Ctrl + C para salir)");

        }

        private static async Task OnUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            if (update.Message is not { } message || message.Text is not { } text)
            {
                return;
            }

            var reply = BuildReply(text);
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id,text: reply,cancellationToken: ct);
        }


        private static Task OnPollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken __)
        {
            Console.WriteLine($"Error de Polling: {exception.Message}");
            return Task.CompletedTask;
        }


        public static bool editMode = false;

        public static string BuildReply(string incomingText)
        {
            var normalized = incomingText.Trim().ToLower();
            if (normalized.Length == 0)
            {
                return "Escribe un mensaje!";
            }

            var key = normalized.ToLowerInvariant();

            if (editMode)
            {
                string[] datosNuevos = key.Split(',');
                editMode = false;
                return datosNuevos[1]; //aca deberia hacer el insert a la base de datos,
                                       //pero por ahora solo devuelve el segundo dato que se ingresa
                                       

            }

            switch (key)
            {
                case "hola":
                    return "BUENAS 1!";
                case "menu":
                    return "1- Ver lista de usuarios, 2- Nuevo Usuario";

                case "1":
                    editMode = false;
                    return SqliteHandler.GetJson("select * from Login");
                case "2":
                    editMode = true;
                    return "";
                default:
                    return $"Recibí tu mensaje: {incomingText}";
            }
        }
    }
}
