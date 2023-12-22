using Balu_Ass_2.BotSettings;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class ExclusiveMainView
    {
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static readonly BotConfig? BotConfig = ProvidedSetups.BotConfig;

        public static async Task SendExclusiveMainView()
        {
            var exclusiveChannel = await Client.GetChannelAsync(BotConfig.ChannelIds.ExclusiveViewChannel);

            DiscordButtonComponent addChildToDb = new(ButtonStyle.Primary, "addChildToDb", "Kind hinzufügen");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier können verschiedene Befehle ausgeführt werden."))
                .AddComponents(addChildToDb);

            await exclusiveChannel.SendMessageAsync(message);
        }
    }
}
