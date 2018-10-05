using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace kryptocoin_master.Classi{

    public class DBConnection
    {
        private DBConnection(){}

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private static MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
           return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=; SSLMode=none", databaseName);
                connection = new MySqlConnection(connstring);
                Logger.WriteLine(LogType.Info,"Mi sto connettendo al Database...");
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Logger.WriteLine(LogType.Info,"Chiudo la connessione al Database...");
            connection.Close();
        }        
    }
    
}