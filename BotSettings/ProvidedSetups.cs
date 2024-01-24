using Balu_Ass_2.Data.Database;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.BotSettings
{
    internal static class ProvidedSetups
    {
        public static ApplicationDbContext? Context {  get; set; }
        public static BotConfig? BotConfig { get; set; }
        public static DiscordClient Client { get; set; }
    }
}
