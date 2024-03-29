﻿using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class LogController
    {
        private static ApplicationDbContext Context {  get; set; }
        private static readonly string logFile = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");

        public static void SetContext(ApplicationDbContext _context)
        {
            Context = _context;
        }

        public async static Task SaveLogMessage(int level, int type, string message)
        {
            switch (level)
            {
                case 1:
                    await AddLogToLogfile(CreateLogMessage(level, type, message));
                    break;
                case 2:
                    await AddLogToDb(level, type, message);
                    break;
                case 3:
                    await AddLogToLogfile(CreateLogMessage(level, type, message));
                    await AddLogToDb(level, type, message);
                    break;
            }
        }

        private static async Task AddLogToLogfile(string logMessage)
        {
            using StreamWriter writer = new(logFile, true);
            await writer.WriteLineAsync(logMessage);
        }

        private static string CreateLogMessage(int level, int type, string message)
        {
            string logMessage =$"{DateTime.Now}" + ", " + level.ToString() + ", " + type.ToString() + ", " + message;
            return logMessage;
        }

        private static async Task AddLogToDb(int level, int type, string message)
        {
            var logDb = new SystemLog
            {
                Level = level,
                Type = type,
                Message = message,
                TimeOfAction = DateTime.Now
            };
            await Context.SystemLogs.AddAsync(logDb);
            await Context.SaveChangesAsync();
        }
    }
}
