using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Modals;
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

        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Children> Childrens { get; set; }
        public DbSet<DeletedChildren> DeletedChildrens { get; set; }

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
