using System.Threading.Tasks;
using Telegram.Bot;

namespace kryptocoin_master.Classi.Comandi{
    
    class ComandoHelloWorld : ComandoBase
    {
        public override string nomeComando => "ciao";

        public override async void eseguiComando(Telegram.Bot.Types.Message messaggio, TelegramBotClient clientBot)
        {
            long chatID = messaggio.Chat.Id;
            int idMessaggio = messaggio.MessageId;
            Task.Run(() => clientBot.SendTextMessageAsync(chatID, "HelloWorld Funzionante!", replyToMessageId: idMessaggio));
        }

    }
}