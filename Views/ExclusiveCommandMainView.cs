using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class ExclusiveCommandMainView : ApplicationCommandModule
    {  
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static readonly BotConfig? BotConfig = ProvidedSetups.BotConfig;

        public static async Task SendExclusiveMainView()
        {
            var exclusiveChannel = await Client.GetChannelAsync(BotConfig.ChannelIds.ExclusiveViewChannel);
            await SupportController.DeleteAllMessages(exclusiveChannel);

            DiscordButtonComponent addChildToDb = new(ButtonStyle.Primary, "addChildToDb", "Kind hinzufügen");
            DiscordButtonComponent deleteChildFromDb = new(ButtonStyle.Danger, "deleteChildFromDb", "Kind entfernen");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier können verschiedene Befehle ausgeführt werden."))
                .AddComponents(addChildToDb)
                .AddComponents(deleteChildFromDb);

            await exclusiveChannel.SendMessageAsync(message);
        }

        public static async Task AddChildToDbModal(ComponentInteractionCreateEventArgs args)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Kind hinzufügen")
                .WithCustomId(args.Interaction.Data.CustomId)
                .AddComponents(new TextInputComponent(label: "Vorname", "firstName", "Vorname des Kindes"))
                .AddComponents(new TextInputComponent(label: "Nachname", "lastName", "Nachname des Kindes"))
                .AddComponents(new TextInputComponent(label: "Mutter", "nameOfMother", "Name der Mutter", required: false))
                .AddComponents(new TextInputComponent(label: "Vater", "nameOfFather", "Name des Vaters", required: false));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task DeleteChildFromDbDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();

            var dropdown = new DiscordSelectComponent("deleteChildFromDbDropdwon", "Kind auswählen", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkRed)
                .WithTitle("Welches Kind soll aus der Liste entfernt werden?"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }
    }
}
