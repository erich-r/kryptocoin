using System;
using System.Collections.Generic;
using System.IO;
using kryptocoin_master.Classi;
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

}