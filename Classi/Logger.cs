using System;
using System.IO;

namespace kryptocoin_master.Classi{

    public static class Logger{

        private static string pathToLog;

        private static FileStream ostrm;
        private static TextWriter writer;
        private static TextWriter oldOut;

        private static int fileCount;
        private static DateTime dateOfStopLoggin;

        private const int FILEMAXROWS = 10;
        private static int fileCurrentrows;

        public static void startLogging(){

            if(!dateOfStopLoggin.Date.Equals(DateTime.Now.Date) || dateOfStopLoggin == null)
                fileCount = 0;
            else{
                //dateOfStopLoggin = DateTime.Now;
                fileCount++;
            }
            
            fileCurrentrows = 0;

            pathToLog = "logs/";
            Console.Write(fileCount);
            pathToLog = pathToLog + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + fileCount +".txt";

            oldOut = Console.Out;
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

        public static void stopLogging(){
            dateOfStopLoggin = DateTime.Now;
            Console.SetOut(oldOut);
            writer.Close();
            
            ostrm.Close();

        }
        public static void WriteLine(LogType logtype,string data){

            Console.WriteLine("{0} | {1} | {2}",DateTime.Now,logtype.Value,data);
            fileCurrentrows++;
            checkFileSize();

        }
        private static void checkFileSize(){
            //Logger.Write(LogType.Debug,fileInfo.Length.ToString());
            if(fileCurrentrows >= FILEMAXROWS){
                
                stopLogging();
                startLogging();

            }

        }
    }

}