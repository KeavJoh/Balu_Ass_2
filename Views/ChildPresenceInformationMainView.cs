using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Views
{
    internal class ChildPresenceInformationMainView
    {
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static readonly BotConfig? BotConfig = ProvidedSetups.BotConfig;

        public static async Task SendPresenceInformationsMainView()
        {
            var presenceInformationChannel = await Client.GetChannelAsync(BotConfig.ChannelIds.ChildPresenceInformationViewChannel);
            await SupportController.DeleteAllMessages(presenceInformationChannel);

            await SendPresenceInformationActualView(presenceInformationChannel);
            await SendPresenceInformationFutureView(presenceInformationChannel);
        }

        private static async Task SendPresenceInformationActualView(DiscordChannel channelId)
        {
            var deregistrationList = _DataStore.DeregistrationList.Where(x => x.DeregistrationDay == DateTime.Now.Date).ToList();
            int GroupOneDeregCount = deregistrationList.Count(x => x.ChildrenGroup == 1);
            int GroupTwoDeregCount = deregistrationList.Count(x => x.ChildrenGroup == 2);

            var embedInitialMessage = new DiscordEmbedBuilder()
            {
                Title = "__Anwesenheit für heute__",
                Color = DiscordColor.Red,
                Timestamp = DateTime.Now,
            };

            var descriptionBuilder = new StringBuilder();

            if (!deregistrationList.Any())
            {
                descriptionBuilder.AppendLine($"**{DateTime.Now.ToString("dd.MM.yyyy")}**");
                descriptionBuilder.AppendLine($"**Angemeldete: {_DataStore.ChildrenTotalCount - deregistrationList.Count}**");
                descriptionBuilder.AppendLine($"Bären: {_DataStore.GroupOneCount - GroupOneDeregCount}");
                descriptionBuilder.AppendLine($"Elefanten: {_DataStore.GroupTwoCount - GroupTwoDeregCount}");
                descriptionBuilder.AppendLine();
                descriptionBuilder.AppendLine($"Es fehlen:");
                descriptionBuilder.AppendLine($"`Für heute sind keine Kinder abgemeldet`");
            }
            else
            {
                descriptionBuilder.AppendLine($"**{DateTime.Now.ToString("dd.MM.yyyy")}**");
                descriptionBuilder.AppendLine($"Angemeldet: {_DataStore.ChildrenTotalCount - deregistrationList.Count()}");
                descriptionBuilder.AppendLine($"Bären: {_DataStore.GroupOneCount - GroupOneDeregCount}");
                descriptionBuilder.AppendLine($"Elefanten: {_DataStore.GroupTwoCount - GroupTwoDeregCount}");
                descriptionBuilder.AppendLine();
                descriptionBuilder.AppendLine($"Es fehlen:");
                foreach (var child in deregistrationList)
                {
                    descriptionBuilder.AppendLine($"`- {child.FirstName} {child.LastName[..1]}. : {child.Reason}`");
                }

                descriptionBuilder.AppendLine($" ");
            }

            embedInitialMessage.Description = descriptionBuilder.ToString();
            await channelId.SendMessageAsync(embedInitialMessage);
        }

        private static async Task SendPresenceInformationFutureView(DiscordChannel channelId)
        {
            var deregistrationList = _DataStore.DeregistrationList
                .Where(d => d.DeregistrationDay.Date > DateTime.Now.Date)
                .GroupBy(d => new
                {
                    Year = d.DeregistrationDay.Year,
                    Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        d.DeregistrationDay, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Week)
                .ToList();

            var embedInitialMessage = new DiscordEmbedBuilder()
            {
                Title = "__Zukünftige Anwesenheiten__",
                Color = DiscordColor.Yellow,
                Timestamp = DateTime.Now,
            };

            var descriptionBuilder = new StringBuilder();

            if (!deregistrationList.Any())
            {
                descriptionBuilder.AppendLine($"`Es gibt keine Abmeldungen für die nächsten Tage`");

                embedInitialMessage.Description = descriptionBuilder.ToString();
                await channelId.SendMessageAsync(embed: embedInitialMessage.Build());
            }
            else
            {
                embedInitialMessage.Description = descriptionBuilder.ToString();
                await channelId.SendMessageAsync(embed: embedInitialMessage.Build());

                foreach (var weeklyGroup in deregistrationList)
                {
                    var embedKwMessage = new DiscordEmbedBuilder()
                    {
                        Title = $"**KW {weeklyGroup.Key.Week}**",
                        Color = DiscordColor.Yellow
                    };

                    var descriptionKwBuilder = new StringBuilder();

                    // Fügt die Kalenderwoche hinzu
                    //descriptionBuilder.AppendLine($"**KW {weeklyGroup.Key.Week}**");
                    descriptionKwBuilder.AppendLine("-----");

                    var dailyGroups = weeklyGroup
                        .OrderBy(d => d.DeregistrationDay)
                        .GroupBy(d => d.DeregistrationDay.Date)
                        .ToList();

                    foreach (var dailyGroup in dailyGroups)
                    {
                        int GroupOneDeregCount = dailyGroup.Count(x => x.ChildrenGroup == 1);
                        int GroupTwoDeregCount = dailyGroup.Count(x => x.ChildrenGroup == 2);

                        string dateHeader = dailyGroup.Key.ToString("__**dddd, dd.MM.yyyy**__", new CultureInfo("de-DE"));
                        descriptionKwBuilder.AppendLine(dateHeader);
                        descriptionKwBuilder.AppendLine($"Angemeldet: {_DataStore.ChildrenTotalCount - dailyGroup.Count()}");
                        descriptionKwBuilder.AppendLine($"Bären: {_DataStore.GroupOneCount - GroupOneDeregCount}");
                        descriptionKwBuilder.AppendLine($"Elefanten: {_DataStore.GroupTwoCount - GroupTwoDeregCount}");
                        descriptionKwBuilder.AppendLine();
                        descriptionKwBuilder.AppendLine($"Es fehlen:");

                        foreach (var deregistration in dailyGroup)
                        {
                            descriptionKwBuilder.AppendLine($"`- {deregistration.FirstName} {deregistration.LastName[..1]}.: {deregistration.Reason ?? "kein Grund angegeben"}`");
                        }

                        descriptionKwBuilder.AppendLine(); // Fügt eine Leerzeile nach den Einträgen eines Tages hinzu
                    }

                    embedKwMessage.Description = descriptionKwBuilder.ToString();
                    await channelId.SendMessageAsync(embed: embedKwMessage.Build());
                }
            }
        }
    }
}
