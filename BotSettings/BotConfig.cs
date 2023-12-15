using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.BotSettings
{
    internal class BotConfig
    {
        public _BotConnectionSettings BotSettings { get; set; }
        public _ChannelIds ChannelIds { get; set; }
        public _DatabaseConnectionString DatabaseConnectionString { get; set; }
    }
}
