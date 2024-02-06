using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            DiscordButtonComponent createVoting = new(ButtonStyle.Primary, "createVoting", "Absitmmung erstellen");
            DiscordButtonComponent editChild = new(ButtonStyle.Primary, "editChild", "Kind bearbeiten");

            var message = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Hallo und Herzlich Wilkommen")
                .WithDescription("Hier können verschiedene Befehle ausgeführt werden.")
                .AddField("Kind hinzufügen", $"`Hier kannst du ein Kind dem Balu hinzufügen`")
                .AddField("Kind entfernen", $"`Hier kannst du ein Kind aus dem Balu entfernen`")
                .AddField("Abstimmung erstellen", "`Hier kannst du eine Abstimmung erstellen`"))
                .AddComponents(addChildToDb)
                .AddComponents(editChild)
                .AddComponents(deleteChildFromDb)
                .AddComponents(createVoting);

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
                .AddComponents(new TextInputComponent(label: "Vater", "nameOfFather", "Name des Vaters", required: false))
                .AddComponents(new TextInputComponent(label: "Gruppe", "group", "1 = Bären / 2 = Elefanten", required: true, max_length: 1));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task EditChildModal(ComponentInteractionCreateEventArgs args)
        {
            int.TryParse(args.Interaction.Data.Values[0], out int childId);
            _DataStore.EditChildId = childId;
            Children selectChild = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == childId);

            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Kind anpassen")
                .WithCustomId("editChildToDb")
                .AddComponents(new TextInputComponent(label: "Vorname", "firstName", value: selectChild.FirstName))
                .AddComponents(new TextInputComponent(label: "Nachname", "lastName", value: selectChild.LastName))
                .AddComponents(new TextInputComponent(label: "Mutter", "nameOfMother", value: selectChild.Mother, required: false))
                .AddComponents(new TextInputComponent(label: "Vater", "nameOfFather", value: selectChild.Father, required: false))
                .AddComponents(new TextInputComponent(label: "Gruppe", "group", value: selectChild.Group.ToString(), required: true, max_length: 1));
            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task DeleteChildFromDbDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();

            var dropdown = new DiscordSelectComponent("deleteChildFromDbDropdwon", "Welches Kind möchtest du entfernen?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Wähle bitte ein Kinde welches aus dem Balu entfernt werden soll:"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }

        public static async Task EditChildDropdown(ComponentInteractionCreateEventArgs args)
        {
            var options = await _DataStore.GetChildrensList();

            var dropdown = new DiscordSelectComponent("editChildDropdown", "Welches Kind möchtest du anpassen?", options);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkBlue)
                .WithTitle("Wähle bitte ein Kind zum anpassen aus:"))
                .AddComponents(dropdown);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message);
        }
    }
}
