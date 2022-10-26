using System;
using System.Collections.Generic;
using System.Text;

namespace Business.CCS
{
    public class FileLogger : ILogger
    {
        public void Log()
        {
            //Console.WriteLine("Dosyaya Loglandı");
            //StringBuilder sb = new StringBuilder();
            //sb.Append("log something");
            //// flush every 20 seconds as you do it
            //File.AppendAllText(filePath + "log.txt", sb.ToString());
            //sb.Clear();
        }
    }
}
