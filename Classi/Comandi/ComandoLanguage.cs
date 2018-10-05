using System;
using System.Threading.Tasks;
using kryptocoin_master.Classi.Comandi;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace kryptocoin_master.Classi{

    class ComandoLanguage : ComandoBase
    {
        public override string nomeComando => "/lan";
        public override bool richiedeParametri => true;

        public override async void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot,params string[]parametri)
        {
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID,"Funziona!!!",replyToMessageId:idMessaggio,parseMode:ParseMode.Html));

            

        }
    }

}