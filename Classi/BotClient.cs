using System;
using System.Collections.Generic;
using System.Linq;
using kryptocoin_master.Classi.Comandi;
using MySql.Data.MySqlClient;
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

        public static long[] getBotAdmins(){

            long[] toRtn = null;
            DBConnection db = DBConnection.Instance();
            MySqlConnection connection = db.Connection;
            
            string query = $"SELECT COUNT(*) FROM UTENTI WHERE isAdmin = 1";
            
            MySqlCommand cmd = new MySqlCommand(query,connection);

            //Logger.WriteLine(LogType.Debug,query + " " + parametri[0]);
            long numAdmins;
            try{
                numAdmins = (long)cmd.ExecuteScalar();
                //Logger.WriteLine(LogType.Info,$"Comando start - ho eseguito la query {query} per inserire l'utente {parametri[0]} ({chatID}) che ha scritto /start");
                Logger.WriteLine(LogType.Info,"BotClient - Sono riuscito nell'ottenere il numero di admin del bot.");
            }
            catch(MySqlException e){
                Logger.WriteLine(LogType.Error,$"BotClient - Errore nell'eseguire la query {query} per ottenere il numero di admin admin, dettagli errore: {e.Message}");
                return null;
            }

            query = $"SELECT * FROM UTENTI WHERE isAdmin = 1";
            
            cmd = new MySqlCommand(query,connection);
            
            toRtn = new long[numAdmins];

            //Logger.WriteLine(LogType.Debug,query + " " + parametri[0]);
            try{
                cmd.ExecuteNonQuery();
                //Logger.WriteLine(LogType.Info,$"Comando start - ho eseguito la query {query} per inserire l'utente {parametri[0]} ({chatID}) che ha scritto /start");
                Logger.WriteLine(LogType.Info,"BotClient - Sono riuscito a ottenere la lista degli admin del bot.");
            }
            catch(MySqlException e){
                Logger.WriteLine(LogType.Error,$"BotClient - Errore nell'eseguire la query {query} per ottenere gli admin, dettagli errore: {e.Message}");
                return null;
            }

            
            using(MySqlDataReader reader = cmd.ExecuteReader()){
                
                int index = 0;
                while(reader.Read()){
                    //Console.WriteLine(index+" BotClient get");
                    toRtn[index] = reader.GetInt64("chatID");
                    index++;
                }

            }

            return toRtn;
        }


        
    }
}