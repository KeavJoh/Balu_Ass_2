using Balu_Ass_2.BotSettings;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class RegistrateBotController
    {
        public  DiscordConfiguration InitClient(BotConfig botConfig)
        {
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = botConfig?.BotConnectionSettings?.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            return discordConfig;
        }
    }
}
