namespace kryptocoin_master.Classi{

    //classe che contiene gli attributi necessari per un qualsiasi bot di telegram
    public class ImpostazioniBot
    {
        
        //nome del bot (dopo la @)
        public static string nome { get; set; } 
        //la chiave necessaria per usufruire delle api di telegram
        public static string chiaveAPI { get; set; } 

        public static void setAttributi(string nomeBot,string chiaveAPIBot){

            nome = nomeBot;
            chiaveAPI = chiaveAPIBot;

        }
    }

}
