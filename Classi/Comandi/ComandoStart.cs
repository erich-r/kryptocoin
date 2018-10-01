using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
namespace kryptocoin_master.Classi.Comandi
{

    class ComandoStart : ComandoBase
    {
        public override string nomeComando => "/start";

        public override async void eseguiComando(Telegram.Bot.Types.Message messaggio, TelegramBotClient clientBot)
        {
            long chatID = messaggio.Chat.Id;
            int idMessaggio = messaggio.MessageId;

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // prima riga
                        {
                            InlineKeyboardButton.WithCallbackData("Italiano"),
                            InlineKeyboardButton.WithCallbackData("English")
                        },
                        new [] // seconda riga
                        {
                            InlineKeyboardButton.WithCallbackData("Español"),
                            InlineKeyboardButton.WithCallbackData("Pусский")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("汉语")
                        }
                    });
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, "Welcome!",replyMarkup:inlineKeyboard));
        }

    }
}