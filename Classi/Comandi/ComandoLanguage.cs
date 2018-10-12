using System;
using System.Threading.Tasks;
using kryptocoin_master.Classi.Comandi;
using MySql.Data.MySqlClient;
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

            string toWrite = "";
            string lingua = "";
            DBConnection db = DBConnection.Instance();
            MySqlConnection connection = db.Connection;
            
            string query = $"SELECT * FROM utenti WHERE chatID = ?chatID;";
            MySqlCommand cmd = new MySqlCommand(query,connection);
            cmd.Parameters.AddWithValue("?chatID",chatID);

            try{
                cmd.ExecuteNonQuery();
                Logger.WriteLine(LogType.Info,$"Comando language - ho eseguito la query {query} per inserire l'utente {parametri[0]} ({chatID}) che ha scritto /start");
            }
            catch(MySqlException e){
                Logger.WriteLine(LogType.Error,$"Comando language - Errore nell'eseguire la query {query}, l'utente è gia presente! messaggio: {e.Message}");
                return;
            }

            using(MySqlDataReader reader = cmd.ExecuteReader()){

                while(reader.Read())
                    lingua = reader.GetString("lingua");

            }
            
            string[] comandoDigitato = parametri[1].Split(" ");
            

            if(comandoDigitato.Length == 1)
                toWrite = LanguageManager.getFrase(lingua,"comandoLinguaSenzaParametro");
            else if(comandoDigitato.Length > 2)
                toWrite = LanguageManager.getFrase(lingua,"comandoLinguaTroppiParametri");
            else if(comandoDigitato.Length==2 && comandoDigitato[1] == "/lan")
                toWrite = LanguageManager.getFrase(lingua,"comandoLinguaParametriOrdinatiMale");
            else{

                string nuovaLingua = comandoDigitato[1].ToLower();
                Logger.WriteLine(LogType.Debug,$"Comando Lan - {nuovaLingua}");

                if(nuovaLingua == "italian" || nuovaLingua == "english"){
                    
                    query = "UPDATE utenti SET lingua=?lingua WHERE chatID=?chatID;";
                    cmd = new MySqlCommand(query,connection);
                    cmd.Parameters.AddWithValue("?chatID",chatID);
                    cmd.Parameters.AddWithValue("?lingua",nuovaLingua);

                    try{
                        cmd.ExecuteNonQuery();
                    }
                    catch(MySqlException e){
                        Logger.WriteLine(LogType.Error,$"ComandoLanguage - Non sono riuscito a cambiare la vecchia lingua {lingua} con la nuova {nuovaLingua} a {parametri[0]} ({chatID}), {e.Message}");
                        toWrite = LanguageManager.getFrase(lingua,"comandoLinguaerroreCambioLingua");
                    }

                    lingua = nuovaLingua;

                    toWrite = LanguageManager.getFrase(lingua,"comandoLinguaEffettuato");

                }
                else{
                    toWrite = LanguageManager.getFrase(lingua,"comandoLinguaNonAncoraSupportata");
                }

            }
            
            await Task.Run(() => clientBot.SendTextMessageAsync(chatID,toWrite,replyToMessageId:idMessaggio,parseMode:ParseMode.Html));

            

        }
    }

}