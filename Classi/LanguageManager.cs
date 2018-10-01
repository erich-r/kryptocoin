using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        foreach (var file in d.GetFiles("*.json"))
        {
            Console.WriteLine(file.Name);
            string dictKey = file.Name.Split('.')[0];
            lingue.Add(dictKey,new Dictionary<string, string>());
            lingue[dictKey] = deserializzaLingua(folderPath,file.Name);
        }

    }

    private Dictionary<string,string> deserializzaLingua(string folderPath,string lingua){
        string fileToDeserialize = string.Concat(folderPath,lingua);
        Console.WriteLine("File : {0}",fileToDeserialize);
        string json;
        json = File.ReadAllText(fileToDeserialize);
        return JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
    }

    public static Dictionary<string,Dictionary<string,string>> getLingue(){

        return lingue;

    }

    public static string getFrase(string lingua,string contesto){

        var qry = from outer in lingue from inner in outer.Value select inner.Value;
        Console.WriteLine(qry.Count() + "Prova");
        return lingue[lingua][contesto];

    }

}