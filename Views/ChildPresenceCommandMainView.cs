using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class ChildPresenceCommandMainView
    {
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static readonly BotConfig? BotConfig = ProvidedSetups.BotConfig;

        public static async Task SendChildPresenceMainView()
        {
            var presenceCommandChannel = await Client.GetChannelAsync(BotConfig.ChannelIds.ChildPresenceViewChannel);
            await SupportController.DeleteAllMessages(presenceCommandChannel);

            DiscordButtonComponent deregistrateChild = new(ButtonStyle.Primary, "deregistrateChild", "Kind Abmelden");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier können verschiedene Befehle ausgeführt werden."))
                .AddComponents(deregistrateChild);

            await presenceCommandChannel.SendMessageAsync(message);
        }

        public static async Task DeregistrateChildDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();
            var dropdown = new DiscordSelectComponent("deregistrateChildDropdown", "Welches Kind möchtest du Abmelden?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Welches Kind möchtest du Abmelden?"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }

        public static async Task DeregistrateChildModal(ComponentInteractionCreateEventArgs args)
        {
            int.TryParse(args.Values[0], out int id);
            _DataStore.ChildId = id;

            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Kind abmelden")
                .WithCustomId("deregistrateChildModal")
                .AddComponents(new TextInputComponent(label: "Von Datum (tt.MM.jjjj / tt.MM.jj)", "dateFrom", "bsp. 20.01.2024", min_length: 8, max_length: 10))
                .AddComponents(new TextInputComponent(label: "Bis Datum (tt.MM.jjjj / tt.MM.jj)", "dateTo", "Nur benötigt für abwesenheiten länger als 1 Tag", min_length: 8, max_length: 10, required: false))
                .AddComponents(new TextInputComponent(label: "Grund", "reason", "Grund der Abwesenheit", max_length: 50));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }
    }
}
