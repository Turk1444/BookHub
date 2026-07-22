using System;
using System.Collections.Generic;
using System.Text;

namespace BookHub.DataAccess.Models
{
    public class LoggerService
    {
        private readonly string _logPath = "logs.txt";

        public void Log(string level, string message)
        {
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToUpper()}] {message}";
            File.AppendAllText(_logPath, entry + Environment.NewLine);
        }
    }
}
