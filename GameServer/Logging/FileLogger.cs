using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Logging
{
    /*
    class FileLogger : ILoggerProvider
    {
        private string m_Path;

        public FileLogger()
        {
            m_Path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "logs";
            if(!Directory.Exists(m_Path))
            {
                Directory.CreateDirectory(m_Path);
            }
        }

        public void Log(string message)
        {
            string path = m_Path + "\\" + DateTime.Today.ToShortDateString() + ".txt";            

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(message);
            }
        }
    }*/
}
