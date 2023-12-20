using Balu_Ass_2.BotSettings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Data.Database
{
    internal class ApplicationDbContext : DbContext
    {
        private readonly BotConfig _botConfig;

        public ApplicationDbContext (BotConfig botConfig)
        {
            _botConfig = botConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_botConfig.DatabaseConnectionString.DefaultDatabaseConnection);
        }
    }
}
