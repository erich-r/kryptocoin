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
            
            string query = $"INSERT INTO utenti (chatID,nome) VALUES (?chatID,?nome);";
            
            MySqlCommand cmd = new MySqlCommand(query,connection);
            cmd.Parameters.AddWithValue("?chatID",chatID);
            cmd.Parameters.AddWithValue("?nome",parametri[0]);

            Logger.WriteLine(LogType.Debug,query + " " + parametri[0]);

            bool utentePresente = false;

            try{
                cmd.ExecuteNonQuery();
                Logger.WriteLine(LogType.Info,$"Comando start - ho eseguito la query {query} per inserire l'utente {parametri[0]} ({chatID}) che ha scritto /start");
            }
            catch(MySqlException e){
                Logger.WriteLine(LogType.Error,$"Comando start - Errore nell'eseguire la query {query}, l'utente è gia presente! messaggio: {e.Message}");
                utentePresente = true;
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

            string lingua = "";
            if(utentePresente){
                //Logger.WriteLine(LogType.Info,$"L'utente {parametri[0]} è gia nel DB, quindi cerco al lingua scelta");
                lingua = LanguageManager.getLinguaUtente(chatID);

                

                Logger.WriteLine(LogType.Info,$"Comando start - L'utente {parametri[0]} è gia nel DB, nel DB ha scelto la lingua {lingua}");


            }

            await Task.Run(() => clientBot.SendTextMessageAsync(chatID, LanguageManager.getFrase(lingua,"avvioBot"),replyMarkup:inlineKeyboard,parseMode:ParseMode.Html));
        }

    }
}