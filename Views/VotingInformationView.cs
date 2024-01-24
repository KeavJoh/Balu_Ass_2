using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class VotingInformationView : ApplicationCommandModule
    {
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static readonly int DeleteTimeSpan = ProvidedSetups.BotConfig.GlobalSettings.DeleteTimeSpan;
        private static int optionCounter;

        private static readonly DiscordEmoji[] emojiOptions = [
            DiscordEmoji.FromName(Client, ":one:"),
            DiscordEmoji.FromName(Client, ":two:"),
            DiscordEmoji.FromName(Client, ":three:"),
            DiscordEmoji.FromName(Client, ":four:")
            ];

        public static async Task CreateVotingModal(ComponentInteractionCreateEventArgs args)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Abstimmung erstellen")
                .WithCustomId(args.Interaction.Data.CustomId)
                .AddComponents(new TextInputComponent(label: "Beschreibung", "description", "Beschreibung"))
                .AddComponents(new TextInputComponent(label: "Option 1", "option1", "Pflichtfeld: Erste Auswahlmöglichkeit", required: true))
                .AddComponents(new TextInputComponent(label: "Option 2", "option2", "Pflichtfeld: Zweite Auswahlmöglichkeit", required: true))
                .AddComponents(new TextInputComponent(label: "Option 3", "option3", "Dritte Auswahlmöglichkeit", required: false))
                .AddComponents(new TextInputComponent(label: "Option 4", "option4", "Vierte Auswahlmöglichkeit", required: false));

            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateVotingAsync(ModalSubmitEventArgs args)
        {
            await CreateVoting(args.Values["description"], args.Values["option1"], args.Values["option2"], args.Values["option3"], args.Values["option4"]);

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                MessageController.CreateInteractionResponseMessage($"Die Abstimmung wurde erfolgreich angelegt und alle Informiert!", 1));

            await Task.Delay(DeleteTimeSpan);
            await args.Interaction.DeleteOriginalResponseAsync();
        }

        private static async Task CreateVoting(string description, string option1, string option2, string option3, string option4)
        {
            var channel = await Client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.VotingInformationViewChannel);

            var voteMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.DarkBlue,
                Title = "Abstimmung",
                Description = description + "\n\n" + GetOptionsDesrcription(option1, option2, option3, option4),
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "Bitte denkt daran, das Ihr für mehrere Optionen abstimmen könnt. Für einige Abstimmungen kann es wichtig sein, das Ihr nur eine Stimme abgebt!"
                }
            };

            var message = await channel.SendMessageAsync(voteMessage);

            for ( var i = 1; i <= optionCounter; i++ )
            {
                await message.CreateReactionAsync(emojiOptions[i -1]);
            }

            await SendVotingInformationMessage();
        }

        private static async Task SendVotingInformationMessage()
        {
            var channel = await Client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.SendVotingInformationChannel);
            var channelVote = await Client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.VotingInformationViewChannel);


            var message = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Blue,
                Title = "Es gibt eine neue Abstimmung!",
                Description = $"Im Channel <#{channelVote.Id}> wurde eine neue Abstimmung erstellt. " +
                    $"Bitte gebt alle eure Stimme ab damit wir ein möglichst genaues Ergebnis bekommen. Vielen Dank!"
            };

            await channel.SendMessageAsync(message);
        }

        private static string GetOptionsDesrcription(string option1, string option2, string option3, string option4)
        {
            string optionsDescription;

            if (option3 == "")
            {
                optionsDescription = $"{emojiOptions[0]} | **{option1}** \n\n" +
                    $"{emojiOptions[1]} | **{option2}** \n\n";
                optionCounter = 2;
            }
            else if (option4 == "")
            {
                optionsDescription = $"{emojiOptions[0]} | **{option1}** \n\n" +
                   $"{emojiOptions[1]} | **{option2}** \n\n" +
                   $"{emojiOptions[2]} | **{option3}** \n\n";
                optionCounter = 3;
            } else
            {
                optionsDescription = $"{emojiOptions[0]} | **{option1}** \n\n" +
                   $"{emojiOptions[1]} | **{option2}** \n\n" +
                   $"{emojiOptions[2]} | **{option3}** \n\n" +
                   $"{emojiOptions[3]} | **{option4}** \n\n";
                optionCounter = 4;
            }

            return optionsDescription;
        }
    }
}
