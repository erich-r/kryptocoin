using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace kryptocoin_master.Classi.Comandi{

    class ComandoFeedback : ComandoBase
    {
        public override string nomeComando => "/feedback";

        public override bool richiedeParametri => true;

        public override void eseguiComando(long chatID, int idMessaggio, TelegramBotClient clientBot, params string[] parametri)
        {

            string lingua = LanguageManager.getLinguaUtente(chatID);
            string toWrite = LanguageManager.getFrase(lingua,"avvisoWIP");

            Task.Run(() => clientBot.SendTextMessageAsync(chatID,toWrite,replyToMessageId:idMessaggio,parseMode:ParseMode.Html));

            //string[] comandoDigitato = parametri[1].Split(" ");

        }
    }

}