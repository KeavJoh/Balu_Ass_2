using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Balu_Ass_2.Controllers
{
    internal class MessageController
    {
        public static DiscordInteractionResponseBuilder CreateInteractionResponseMessage(string message, int confirmType)
        {
            string title = "Unbekannter Fehler!";
            DiscordColor color = DiscordColor.White;
            switch (confirmType)
            {
                case 1:
                    {
                        color = DiscordColor.DarkGreen;
                        title = "Vorgang erfolgreich";
                        break;
                    }
                case 2:
                    {
                        color = DiscordColor.Yellow;
                        title = "Vorgang nicht nötig!";
                        break;
                    }
                case 3:
                    {
                        color = DiscordColor.DarkRed;
                        title = "Das hat leider nicht geklappt!";
                        break;
                    }
            }
            return new DiscordInteractionResponseBuilder()
             .AddEmbed(new DiscordEmbedBuilder().WithColor(color).WithTitle(title).WithDescription(message));
        }
    }
}
