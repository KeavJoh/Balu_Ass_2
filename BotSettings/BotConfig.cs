using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.BotSettings
{
    internal class BotConfig
    {
        public BotConnectionSettings? BotConnectionSettings { get; set; }
        public ChannelIds? ChannelIds { get; set; }
        public DatabaseConnectionString? DatabaseConnectionString { get; set; }
    }
}
