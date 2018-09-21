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

        private static DateTime startTime;

        public static void Main(string[] args)
        {
            startTime = DateTime.Now;

            BotClient.setClient(new TelegramBotClient(ImpostazioniBot.chiaveAPI));
            BotClient.setCommands();
            User me = BotClient.getClient().GetMeAsync().Result;
            Console.Title = me.Username;

            BotClient.getClient().OnMessage += e_MessageReceived;
            BotClient.getClient().OnMessageEdited += e_MessageEdited;
            BotClient.getClient().OnCallbackQuery += e_CallbackQuery;
            BotClient.getClient().OnInlineQuery += e_InlineQuery;
            BotClient.getClient().OnInlineResultChosen += e_InlineResult;
            BotClient.getClient().OnReceiveError += e_Error;
            //BotClient.getClient().OnUpdate += e_Update;
            ImpostazioniBot.nome = me.Username;
            BotClient.getClient().StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            
            Console.ReadLine();
            int secondi = 0;
            DateTime fineProgramma = DateTime.Now;
            secondi = fineProgramma.Subtract(startTime).Seconds;
            Console.WriteLine("Programma chiuso dopo {0} secondi",secondi);
            BotClient.getClient().StopReceiving();
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
            throw new NotImplementedException();
        }

        private static void e_MessageEdited(object sender, MessageEventArgs e)
        {
            int messageID = e.Message.MessageId;
            long chatId = e.Message.Chat.Id;
            inviaMessaggio(BotClient.getClient(), "Prova con reply", chatId,messageID);
        }

        private static async void inviaMessaggio(TelegramBotClient client,string testoMessaggio, long chatIdDestinatario, int messageId = 0)
        {

            await Task.Run(() => client.SendTextMessageAsync(chatIdDestinatario, testoMessaggio, replyToMessageId: messageId));
        }

    }
}
