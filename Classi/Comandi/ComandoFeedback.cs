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
            string toWrite = LanguageManager.getFrase(lingua,"errore");

            string messaggio = parametri[1];

            string[] messaggioArray = messaggio.Split(" ");

            if(messaggioArray[0]!= this.nomeComando){
                toWrite = LanguageManager.getFrase(lingua,"comandoFeedbackSbagliato");
            }
            else{
            

                try{
                    foreach(long adminChatID in BotClient.getBotAdmins()){
                        string linguaAdmin = LanguageManager.getLinguaUtente(adminChatID);
                        toWrite = LanguageManager.getFrase(linguaAdmin,"segnalazione");
                        toWrite += $"\n<b>{parametri[0]}</b> ({chatID})\n" + messaggio;
                        Task.Run(() => clientBot.SendTextMessageAsync(adminChatID,toWrite,parseMode:ParseMode.Html));


                    }
                }
                catch(NullReferenceException e){

                    Logger.WriteLine(LogType.Info,$"ComandoFeedback - Errore nell'inviare la segnalazione, errore:{e.Message}");
                    Task.Run(() => clientBot.SendTextMessageAsync(chatID,toWrite,parseMode:ParseMode.Html));
                    return;

                }

            }
            Task.Run(() => clientBot.SendTextMessageAsync(chatID,LanguageManager.getFrase(lingua,"segnalazioneEffettuata"),parseMode:ParseMode.Html));

            //string[] comandoDigitato = parametri[1].Split(" ");

        }



    }

}