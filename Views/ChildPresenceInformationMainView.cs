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

            var embedInitialMessage = new DiscordEmbedBuilder()
            {
                Title = "__Abmedlungen für heute__",
                Color = DiscordColor.Red,
                Timestamp = DateTime.Now,
            };

            var descriptionBuilder = new StringBuilder();

            if (!deregistrationList.Any())
            {
                descriptionBuilder.AppendLine($"**{DateTime.Now.ToString("dd.MM.yyyy")}**");
                descriptionBuilder.AppendLine($" ");
                descriptionBuilder.AppendLine($"`Für heute sind keine Kinder abgemeldet`");
            }
            else
            {
                descriptionBuilder.AppendLine($"**{DateTime.Now.ToString("dd.MM.yyyy")}**");
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
                Title = "__Zukünftige Abmeldungen__",
                Color = DiscordColor.Yellow,
                Timestamp = DateTime.Now,
            };

            var descriptionBuilder = new StringBuilder();

            if (!deregistrationList.Any())
            {
                descriptionBuilder.AppendLine($"**Es gibt keine Abmeldungen für die nächsten Tage**");
            }
            else
            {
                foreach (var weeklyGroup in deregistrationList)
                {
                    // Fügt die Kalenderwoche hinzu
                    descriptionBuilder.AppendLine($"**KW {weeklyGroup.Key.Week}**");
                    descriptionBuilder.AppendLine("-----");

                    var dailyGroups = weeklyGroup
                        .OrderBy(d => d.DeregistrationDay)
                        .GroupBy(d => d.DeregistrationDay.Date)
                        .ToList();

                    foreach (var dailyGroup in dailyGroups)
                    {
                        string dateHeader = dailyGroup.Key.ToString("**dddd, dd.MM.yyyy**", new CultureInfo("de-DE"));
                        descriptionBuilder.AppendLine(dateHeader);

                        foreach (var deregistration in dailyGroup)
                        {
                            descriptionBuilder.AppendLine($"`- {deregistration.FirstName} {deregistration.LastName[..1]}.: {deregistration.Reason ?? "kein Grund angegeben"}`");
                        }

                        descriptionBuilder.AppendLine(); // Fügt eine Leerzeile nach den Einträgen eines Tages hinzu
                    }
                }
            }

            embedInitialMessage.Description = descriptionBuilder.ToString();
            await channelId.SendMessageAsync(embed: embedInitialMessage.Build());
        }
    }
}
