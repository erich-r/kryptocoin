using System;
using System.Threading.Tasks;
using kryptocoin_master.Classi;
using kryptocoin_master.Classi.Comandi;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace kryptocoin_master
{
    class Program
    {

        //private static DateTime startTime;
        private static Logger logger;
        public static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            logger = new Logger("logs/prova.txt");
            logger.startLogging();
            //db
            Logger.Write(LogType.Info,"Connessione al DB");
            DBConnection dBConnection = DBConnection.Instance();
            dBConnection.DatabaseName = "information_schema";

            //try{
            //    if(dBConnection.IsConnect()){
            //        Console.WriteLine("Connessione riuscita!");
            //    }
            //}
            //catch(MySql.Data.MySqlClient.MySqlException e){
            //    Console.WriteLine("Non sono riuscito a connettermi al database, termino il programma.. \nInformazioni errore: {0}",e.Message);
            //    abort = true;
            //}
            

            

            User me = null;
                
            try{
                BotClient.setClient(new TelegramBotClient(ImpostazioniBot.chiaveAPI));
                BotClient.setCommands();
               me = BotClient.getClient().GetMeAsync().Result;
            }                
            catch(AggregateException e){
                Console.WriteLine("Errore nel reperire le informazioni del bot, termino il programma.\nInformazioni errore: {0}",e.Message);
                chiudiProgramma(startTime);
            }
            

            
            try{
                BotClient.getClient().StartReceiving(Array.Empty<UpdateType>());
                Console.WriteLine($"Start listening for @{me.Username}");
            }
            catch(Exception e){
                Console.WriteLine("Errore: {0}",e.Message);
                chiudiProgramma(startTime);
            }
            
            if(BotClient.getClient() == null){
                chiudiProgramma(startTime);
            }
            
                
            LanguageManager lm = new LanguageManager();

            Console.Title = me.Username;
            
            BotClient.getClient().OnMessage += e_MessageReceived;
            BotClient.getClient().OnMessageEdited += e_MessageEdited;
            BotClient.getClient().OnCallbackQuery += e_CallbackQuery;
            BotClient.getClient().OnInlineQuery += e_InlineQuery;
            BotClient.getClient().OnInlineResultChosen += e_InlineResult;
            BotClient.getClient().OnReceiveError += e_Error;
                //BotClient.getClient().OnUpdate += e_Update;
            ImpostazioniBot.nome = me.Username;
            Console.ReadLine();
            BotClient.getClient().StopReceiving();
            
            chiudiProgramma(startTime);
            
        }

        //private static void e_Update(object sender, UpdateEventArgs e)
        //{
        //    Console.WriteLine("Prova");
        //}

        private static void e_MessageReceived(object sender, MessageEventArgs e)
        {
            string username = e.Message.Chat.FirstName;
            long id = e.Message.Chat.Id;
            string messagge = e.Message.Text;

            //debug

            Console.Write("{0} ({1}) ha scritto {2}", username, id, messagge);

            ComandoBase comandoDaEseguire = BotClient.comandoDigitato(e.Message);

            if (comandoDaEseguire != null)
            {
                Console.WriteLine(" - Eseguo il comando {0}", comandoDaEseguire.nomeComando);
                comandoDaEseguire.eseguiComando(e.Message, BotClient.getClient());
            }
            else
                Console.WriteLine("");
            

        }

        private static void e_Error(object sender, ReceiveErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void e_InlineResult(object sender, ChosenInlineResultEventArgs e)
        {
            throw new NotImplementedException();

        }

        private static void e_InlineQuery(object sender, InlineQueryEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void e_CallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine(e.CallbackQuery.Data);
        }

        private static void e_MessageEdited(object sender, MessageEventArgs e)
        {
            int messageID = e.Message.MessageId;
            long chatId = e.Message.Chat.Id;
            inviaMessaggio(BotClient.getClient(), LanguageManager.getFrase("Italiano","Editato"), chatId,messageID);
        }

        private static async void inviaMessaggio(TelegramBotClient client,string testoMessaggio, long chatIdDestinatario, int messageId = 0)
        {

            await Task.Run(() => client.SendTextMessageAsync(chatIdDestinatario, testoMessaggio, replyToMessageId: messageId));
        }

        private static void chiudiProgramma(DateTime startTime){

            //int secondi = 0;
            
            DateTime endTime = DateTime.Now;
            //secondi = fineProgramma.Subtract(startTime).Seconds;
            //Console.WriteLine("Programma chiuso dopo {0} secondi",secondi);
            Console.WriteLine("Programma iniziato il {0} e terminato il {1}",startTime,endTime);
            logger.stopLogging();
            Environment.Exit(1);

        }

    }
}
