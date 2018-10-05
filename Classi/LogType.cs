using System;

namespace kryptocoin_master.Classi{

    public class LogType{
        private LogType(string value) { Value = value; }

        public string Value { get; set; }
        public static LogType Debug { get { return new LogType("[Debug]"); } }
        public static LogType Info { get { return new LogType("[Info]"); } }
        public static LogType Warning { get { return new LogType("[Warning]"); } }
        public static LogType Error { get { return new LogType("[Error]"); } }

        public static LogType Update { get { return new LogType("[Update]"); } }
    }
}