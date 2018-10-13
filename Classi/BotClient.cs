using System.Collections.Generic;
using System.Linq;
using kryptocoin_master.Classi.Comandi;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace kryptocoin_master.Classi{

    static class BotClient
    {
        //la classe utility che ci permette di istanziare il bot con i suoi metodi

        private static TelegramBotClient client;


        //lista dove verranno aggiunti/rimossi comandi relativi al bot
        private static List<ComandoBase> listaComandi = new List<ComandoBase>();
        //lista readonly per evitare di modificare i comandi
        public static IReadOnlyList<ComandoBase> comandi { get => listaComandi.AsReadOnly(); }
        //metodo asincrono che ritorna il client in esecuzione, altrimenti ne istanzia uno nuovo.
        public static TelegramBotClient getClient()
        {
            return client;
        }

        public static void setClient(TelegramBotClient newClient)
        {
            client = newClient;
        }

        public static ComandoBase comandoDigitato(string input)
        {
            bool trovato = false;
            int i = 0;

            ComandoBase cmdToRtn = null;

            //if (messaggio.Type != Telegram.Bot.Types.Enums.MessageType.Text) return cmdToRtn;

            while (!trovato && i < comandi.Count)
            {
                ComandoBase comando = comandi.ElementAt(i);
                if (comando.verificaComando(input))
                {
                    cmdToRtn = comando;
                    trovato = true;
                }
                else
                    i++;
            }

            return cmdToRtn;
        } 
        
        public static void setCommands()
        {
            listaComandi.Add(new ComandoHelloWorld());
            listaComandi.Add(new ComandoStart());
            listaComandi.Add(new ComandoLanguage());
            listaComandi.Add(new ComandoFeedback());
            Logger.WriteLine(LogType.Info,"Ho fornito i comandi da eseguire al BOT");
        }
        
    }
}