using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace kryptocoin_master.Classi{

    class APIUpdater{

        public string apiUrl {get;set;}

        
        public static async Task aggiornaApi(CancellationToken token,TimeSpan intervalDelay){

            while(true){

                //JsonConverter jsonConverter = new JsonConverter();
                //deseralizza
                //deserializzaApi();
                Logger.WriteLineAsync(LogType.Debug,"Prova da API UPDATER");
                await Task.Delay(intervalDelay);
                if(token.IsCancellationRequested)
                    break;
            }

        }

        private static void deserializzaApi(){
            
        }
    }

}