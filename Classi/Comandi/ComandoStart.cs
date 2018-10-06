using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace kryptocoin_master.Classi.Comandi
{

    class ComandoStart : ComandoBase
    {
        public override string nomeComando => "/start";
        public override bool richiedeParametri => false;

        public override async void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot,params string[]parametri)
        {

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // prima riga
                        {
                            InlineKeyboardButton.WithCallbackData("🇮🇹 Italiano","/lan italian"),
                            InlineKeyboardButton.WithCallbackData("🇬🇧 English","/lan english")
                        },
                        new [] // seconda riga
                        {
                            InlineKeyboardButton.WithCallbackData("🇪🇸 Español","/lan spanish"),
                            InlineKeyboardButton.WithCallbackData("🇷🇺 Pусский","/lan russian")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🇨🇳 汉语","/lan chinese")
                        }
                    });
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, LanguageManager.getFrase("Inglese","avvioBot"),replyMarkup:inlineKeyboard,parseMode:ParseMode.Html));
        }

    }
}