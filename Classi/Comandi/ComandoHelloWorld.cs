using System.Threading.Tasks;
using Telegram.Bot;

namespace kryptocoin_master.Classi.Comandi{
    
    class ComandoHelloWorld : ComandoBase
    {
        public override string nomeComando => "/ciao";

        public override async void eseguiComando(Telegram.Bot.Types.Message messaggio, TelegramBotClient clientBot)
        {
            long chatID = messaggio.Chat.Id;
            int idMessaggio = messaggio.MessageId;
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, LanguageManager.getFrase("Italiano","Saluto"), replyToMessageId: idMessaggio));
        }

    }
}