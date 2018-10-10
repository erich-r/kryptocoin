using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

using kryptocoin_master.Classi;
using MySql.Data.MySqlClient;

namespace kryptocoin_master.Classi.Comandi
{

    class ComandoStart : ComandoBase
    {
        public override string nomeComando => "/start";
        public override bool richiedeParametri => false;

        public override async void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot,params string[]parametri)
        {
            
            DBConnection db = DBConnection.Instance();
            MySqlConnection connection = db.Connection;
            
            string query = $"INSERT INTO utenti (chatID,nome) VALUES ('{chatID}','{parametri[0]}')";
            MySqlCommand cmd = new MySqlCommand(query,connection);

            try{
                cmd.ExecuteNonQuery();
                Logger.WriteLine(LogType.Info,$"Comando start: ho eseguito la query {query} per inserire l'utente {parametri[0]} ({chatID}) che ha scritto /start");

            }
            catch(MySqlException e){
                Logger.WriteLine(LogType.Error,$"Errore nell'eseguire la query {query}, messaggio: {e.Message}");
            }
            

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new [] // prima riga
                        {
                            InlineKeyboardButton.WithCallbackData("🇮🇹 Italiano","/lan italian"),
                            InlineKeyboardButton.WithCallbackData("🇬🇧 English","/lan english")
                        },
                        new [] // seconda riga
                        {
                            InlineKeyboardButton.WithCallbackData("🇪🇸 Español","/lan spanish"),
                            InlineKeyboardButton.WithCallbackData("🇷🇺 Pусский","/lan russian")
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("🇨🇳 汉语","/lan chinese")
                        }
                    });
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, LanguageManager.getFrase("Inglese","avvioBot"),replyMarkup:inlineKeyboard,parseMode:ParseMode.Html));
        }

    }
}