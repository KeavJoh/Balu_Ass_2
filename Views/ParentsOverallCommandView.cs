using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class ParentsOverallCommandView
    {
        private static readonly DiscordClient Client = ProvidedSetups.Client;

        public static async Task SendParentsOverallCommandView()
        {
            var channelId = await Client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.ParentsOverallCommandView);
            await SupportController.DeleteAllMessages(channelId);

            DiscordButtonComponent createVoting = new(ButtonStyle.Primary, "createVoting", "Absitmmung erstellen");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier können verschiedene Befehle ausgeführt werden.")
                .AddField("Abstimmung erstellen", "`Hier kannst du eine Abstimmung erstellen`"))
                .AddComponents(createVoting);

            await channelId.SendMessageAsync(message);
        }
    }
}
