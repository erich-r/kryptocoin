using System;
using System.IO;

namespace kryptocoin_master.Classi{

    public class Logger{

        private FileStream ostrm;
        private StreamWriter writer;

        TextWriter oldOut;

        private bool isLogging;

        private string pathToLog;

        public Logger(string pathToLog){

            isLogging = false;
            this.pathToLog = pathToLog;

        }

        public void startLogging(){

            if(isLogging)
                return;
            
            isLogging = true;
            this.oldOut = Console.Out;
            try
            {
                ostrm = new FileStream (pathToLog, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter (ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine ("Non sono riuscito ad aprire e/o creare il file {0}.\nDettagli errore: {1}",pathToLog,e.Message);
                Environment.Exit(1);
            }
            Console.SetOut (writer);

        }

        public void stopLogging(){
            if(!isLogging)
                return;
            isLogging = false;
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();

        }

        public static void Write(LogType logtype,string data){

            Console.WriteLine("{0} | {1} - {2}",DateTime.Now,logtype.Value,data);

        }
    }

}