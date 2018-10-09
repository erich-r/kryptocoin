using System;
using System.Threading;
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
        public static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            Logger.startLogging();
            TimeSpan intervallo = new TimeSpan(0,0,1);
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            esecuzioneAsincronaIntervalliRegolari(intervallo,token);
            //db
            //Logger.WriteLine(LogType.Info,"Connessione al DB");
            //DBConnection dBConnection = DBConnection.Instance();
            //dBConnection.DatabaseName = "information_schema";

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
                Logger.WriteLine(LogType.Error,$"Errore nel reperire le informazioni del bot, termino il programma.\nInformazioni errore: {e.Message}");
                chiudiProgramma(startTime,source);
            }
            

            
            try{
                BotClient.getClient().StartReceiving(Array.Empty<UpdateType>());
                Logger.WriteLine(LogType.Info,$"Start listening for @{me.Username}");
            }
            catch(Exception e){
                Logger.WriteLine(LogType.Error,$"Errore: {e.Message}");
                chiudiProgramma(startTime,source);
            }
            
            if(BotClient.getClient() == null){
                chiudiProgramma(startTime,source);
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
            
            chiudiProgramma(startTime,source);
            
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
            string toWrite = $"{username} ({id}) ha scritto {messagge}";

            ComandoBase comandoDaEseguire = BotClient.comandoDigitato(e.Message.Text);

            eseguiComando(comandoDaEseguire,e.Message,toWrite);
            

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
            //qui vengono le risposte alle tastiere
            //Logger.WriteLine(LogType.Debug,e.CallbackQuery.Data);
            string tastieraInlineID = e.CallbackQuery.InlineMessageId;
            string nomeTastoPremuto = e.CallbackQuery.ChatInstance;
            string nome = e.CallbackQuery.Message.Chat.FirstName;
            long chatID = e.CallbackQuery.Message.Chat.Id;

            string toWrite = $"{nome} ({chatID}) ha premuto il tasto {nomeTastoPremuto} della tastiera ({tastieraInlineID})";
            
            ComandoBase comandoDaEseguire = BotClient.comandoDigitato(e.CallbackQuery.Data);
            Logger.WriteLine(LogType.Debug,e.CallbackQuery.Message.Text);
            eseguiComando(comandoDaEseguire,e.CallbackQuery.Message,toWrite);

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

        private static void chiudiProgramma(DateTime startTime,CancellationTokenSource source){

            //int secondi = 0;
            
            DateTime endTime = DateTime.Now;
            source.Cancel();
            //secondi = fineProgramma.Subtract(startTime).Seconds;
            //Console.WriteLine("Programma chiuso dopo {0} secondi",secondi);
            Logger.WriteLine(LogType.Info,$"Programma iniziato il {startTime} e terminato il {endTime}");
            Logger.stopLogging();
            Environment.Exit(1);

        }

        private static void eseguiComando(ComandoBase comandoDaEseguire,Message m,string toWrite){

            if (comandoDaEseguire != null)
            {
                toWrite += $" - Eseguo il comando {comandoDaEseguire.nomeComando}";
                //Logger.Write($" - Eseguo il comando {comandoDaEseguire.nomeComando}");
                comandoDaEseguire.eseguiComando(m.Chat.Id,m.MessageId, BotClient.getClient());
            }

            Logger.WriteLine(LogType.Update,toWrite);

        }

        private static async Task esecuzioneAsincronaIntervalliRegolari(TimeSpan intervallo,CancellationToken token){

            while(true){

                Logger.WriteLineAsync(LogType.Debug,"Prova a intervalli regolari asincroni");
                await Task.Delay(intervallo); 
                if(token.IsCancellationRequested)
                    break;
            }
        }



        

    }
}
