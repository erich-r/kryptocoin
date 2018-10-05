using Telegram.Bot;
using Telegram.Bot.Types;

namespace kryptocoin_master.Classi.Comandi{

    abstract class ComandoBase
    {

        public abstract string nomeComando { get; }
        public abstract bool richiedeParametri { get; }
        public abstract void eseguiComando(long chatID,int idMessaggio, TelegramBotClient clientBot);
        public bool verificaComando(string comandoDaControllare)
        {
            bool toRtn = false;
            string[] parole = comandoDaControllare.Split(' ');

            if(!richiedeParametri && parole.Length == 1 && comandoDaControllare.Contains(this.nomeComando))
                toRtn = true;
            else if(richiedeParametri && parole.Length > 1 && comandoDaControllare.Contains(this.nomeComando))
                toRtn = true;

            return toRtn;
        }
    }

}