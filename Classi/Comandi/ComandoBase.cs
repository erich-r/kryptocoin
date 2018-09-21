using Telegram.Bot;
using Telegram.Bot.Types;

namespace kryptocoin_master.Classi.Comandi{

    abstract class ComandoBase
    {

        public abstract string nomeComando { get; }
        public abstract void eseguiComando(Message messaggio, TelegramBotClient clientBot);
        public bool verificaComando(string comandoDaControllare)
        {
            string[] parole = comandoDaControllare.Split(' ');
            return parole.Length == 1 && comandoDaControllare.Contains(this.nomeComando);
        }
    }

}