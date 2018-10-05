using System;
using System.Threading.Tasks;
using kryptocoin_master.Classi.Comandi;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace kryptocoin_master.Classi{

    class ComandoLanguage : ComandoBase
    {
        public override string nomeComando => "/lan";
        public override bool richiedeParametri => true;

        public override void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot)
        {
            Task.Run(() => clientBot.SendTextMessageAsync(chatID,"Funziona!!!",replyToMessageId:idMessaggio));
        }
    }

}