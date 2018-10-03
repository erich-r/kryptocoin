using System;
using System.Collections.Generic;
using System.IO;
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

    }

    private Dictionary<string,string> deserializzaLingua(string folderPath,string lingua){
        string fileToDeserialize = string.Concat(folderPath,lingua);
        Console.WriteLine("Mi preparo a deserializzare il file {0}",fileToDeserialize);
        string json = "";

        try{
            json = File.ReadAllText(fileToDeserialize);
        }
        catch(Exception e){
            Console.WriteLine("Non sono riuscito a deserializzare il file {0}.\nDettagli errore:{1}",fileToDeserialize,e.Message);
            Environment.Exit(1);
        }

        return JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
    }

    public static Dictionary<string,Dictionary<string,string>> getLingue(){

        return lingue;

    }

    public static string getFrase(string lingua,string contesto){

        return lingue[lingua][contesto];

    }

}