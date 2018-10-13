using System;
using System.Collections.Generic;
using System.IO;
using kryptocoin_master;
using kryptocoin_master.Classi;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
public class LanguageManager{

    private static Dictionary<string,Dictionary<string,string>> lingue;

    public LanguageManager(){

        lingue = new Dictionary<string, Dictionary<string, string>>();
        deserializeData();

    }

    private void deserializeData(){


        string folderPath = "Languages/";
        DirectoryInfo d = new DirectoryInfo(folderPath);
        FileInfo[] files = null;

        try{
            files = d.GetFiles("*.json");
        }
        catch(DirectoryNotFoundException e){
            Console.WriteLine($"Errore! directory {folderPath} non trovata! Dettagli errore: {e.Message}");
            Environment.Exit(1);
        }

        foreach (FileInfo file in d.GetFiles("*.json"))
        {
            //Console.WriteLine("Mi preparo a deserializzare il file {0}",file.Name);
            string dictKey = file.Name.Split('.')[0];
            lingue.Add(dictKey,new Dictionary<string, string>());
            lingue[dictKey] = deserializzaLingua(folderPath,file.Name);
        }

        Logger.WriteLine(LogType.Info,"Ho finito di deserializzare i .json delle lingue");

    }

    private Dictionary<string,string> deserializzaLingua(string folderPath,string lingua){
        string fileToDeserialize = string.Concat(folderPath,lingua);
        Logger.WriteLine(LogType.Info,$"Mi preparo a deserializzare il file {fileToDeserialize}");
        string json = "";

        try{
            json = File.ReadAllText(fileToDeserialize);
        }
        catch(Exception e){
            Logger.WriteLine(LogType.Error,$"Non sono riuscito a deserializzare il file {fileToDeserialize}.\nDettagli errore:{e.Message}");
            Environment.Exit(1);
        }

        Dictionary<string,string> toRtn = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
        Logger.WriteLine(LogType.Info,$"Sono riuscito a deserializzare il file {fileToDeserialize}");
        return toRtn;
    }

    public static Dictionary<string,Dictionary<string,string>> getLingue(){

        return lingue;

    }

    public static string getFrase(string lingua,string contesto){

        return lingue[lingua][contesto];

    }

    public static string getLinguaUtente(long chatIdUtente){

        //in caso non riuscissi a trovare la lingua uso l'inlese come default
        string toRtn = "english";
        string query = "SELECT lingua FROM utenti WHERE chatID = ?chatID";
        MySqlCommand cmd;

        MySqlConnection connection = DBConnection.Instance().Connection;

        cmd = new MySqlCommand(query,connection);
        cmd.Parameters.AddWithValue("?chatID",chatIdUtente);
        try{
            cmd.ExecuteNonQuery();
        }
        catch(MySqlException e){
            Logger.WriteLine(LogType.Error,$"Language Manager - Errore nell'ottenere la lingua dell'utente {chatIdUtente}, errore nel dettaglio: {e.Message}");
            return toRtn;
        }
        using(MySqlDataReader reader = cmd.ExecuteReader()){

            while(reader.Read())
                toRtn = reader.GetString("lingua");

        }
        
        return toRtn;

    }

}