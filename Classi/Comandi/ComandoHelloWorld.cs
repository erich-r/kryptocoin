using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
namespace kryptocoin_master.Classi.Comandi{
    
    class ComandoHelloWorld : ComandoBase
    {
        public override string nomeComando => "/ciao";
        public override bool richiedeParametri => false;

        public override async void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot)
        {
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, LanguageManager.getFrase("Italiano","Saluto"), replyToMessageId: idMessaggio,parseMode:ParseMode.Html));
        }

    }
}