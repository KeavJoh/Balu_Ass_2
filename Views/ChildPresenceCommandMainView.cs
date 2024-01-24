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

            DiscordButtonComponent deregistrateChild = new(ButtonStyle.Danger, "deregistrateChild", "Kind Abmelden");
            DiscordButtonComponent fastDeregistrateChild = new(ButtonStyle.Danger, "fastDeregistrateChild", "Schnellabmeldung");
            DiscordButtonComponent registrateChild = new(ButtonStyle.Primary, "registrateChild", "Anmeldung");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier kannst du dein Kind Abmelden oder auch wieder Anmelden. Klicke dazu einfach auf einen der unten stehenden Befehle.")
                .AddField("Abmelden", $"`Hier kannst du dein Kind für einen oder mehrere Tage abmelden`")
                .AddField("Schnellabmeldung", $"`Hier kannst du dein Kind für den aktuellen Tag abmelden`")
                .AddField("Anmelden", $"`Hier kannst du eine Abmeldung für einen oder mehrere Tage rückgängig machen`"))
                .AddComponents(deregistrateChild)
                .AddComponents(fastDeregistrateChild)
                .AddComponents(registrateChild);

            await presenceCommandChannel.SendMessageAsync(message);
        }

        public static async Task DeregistrateChildDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();
            var dropdown = new DiscordSelectComponent("deregistrateChildDropdown", "Welches Kind möchtest du Abmelden?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Wähle bitte das Kind welches du Abmelden möchtest:"))
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

        public static async Task FastDeregistrateChildDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();
            var dropdown = new DiscordSelectComponent("fastDeregistrateChildDropdown", "Welches Kind möchtest du Abmelden?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Wähle bitte das Kind welches du Abmelden möchtest:"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }

        public static async Task RegistrateChildDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();
            var dropdown = new DiscordSelectComponent("registrateChildDropdown", "Welches Kind möchtest du Anmelden?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Wähle bitte das Kind welches du Anmelden möchtest:"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }

        public static async Task RegistrateChildModal(ComponentInteractionCreateEventArgs args)
        {
            int.TryParse(args.Values[0], out int id);
            _DataStore.ChildId = id;

            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Kind anmelden")
                .WithCustomId("registrateChildModal")
                .AddComponents(new TextInputComponent(label: "Von Datum (tt.MM.jjjj / tt.MM.jj)", "dateFrom", "bsp. 20.01.2024", min_length: 8, max_length: 10))
                .AddComponents(new TextInputComponent(label: "Bis Datum (tt.MM.jjjj / tt.MM.jj)", "dateTo", "Nur benötigt für anmeldungen mehr als 1 Tag", min_length: 8, max_length: 10, required: false));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

    }
}
