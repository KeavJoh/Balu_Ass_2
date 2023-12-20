using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class LogController
    {
        public async static Task CreateLogMessage(int level, int type, string message)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string logFile = Path.Combine(currentDirectory, "log.txt");
            using StreamWriter writer = new(logFile, true);

            switch (level)
            {
                case 1:
                    await writer.WriteLineAsync(message);
                    break;
                case 2:
                    //log only DB
                    break;
                case 3:
                    await writer.WriteLineAsync(message);
                    //log also DB
                    break;
            }
        }
    }
}
