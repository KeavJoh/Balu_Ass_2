using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using Balu_Ass_2.Validations;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using DSharpPlus.Entities;

namespace Balu_Ass_2.Handler
{
    internal class ChildDeregistrationTableHandler
    {
        private static readonly int DeleteTimeSpan = ProvidedSetups.BotConfig.GlobalSettings.DeleteTimeSpan;
        private static readonly ApplicationDbContext Context = ProvidedSetups.Context;
        private static readonly DiscordClient Client = ProvidedSetups.Client;

        public static async Task DeregistrateChildToDbHandler(ModalSubmitEventArgs args)
        {
            DiscordMember currentMember = SupportController.GetCurrentUser(args.Interaction.User);
            var selectChild = args.Values;
            var selectChildId = _DataStore.ChildId;
            var firstName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == selectChildId).FirstName;
            var lastName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == selectChildId).LastName;
            DateTime dateFrom = SupportController.ParseStringToDateTime(selectChild["dateFrom"]);
            DateTime dateTo = SupportController.ParseStringToDateTime(selectChild["dateTo"]);

            try
            {
                if (DateTimeValidations.DateTimeInThePast(dateFrom))
                {
                    await LogController.SaveLogMessage(1, 2, $"Das angegebene Startdatum {dateFrom} liegt in der Vergangenheit");
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        MessageController.CreateInteractionResponseMessage($"Das angegebene Datum {dateFrom.ToString("dd.MM.yyyy")} liegt in der Vergangenheit!", 2));

                    await Task.Delay(DeleteTimeSpan);
                    await args.Interaction.DeleteOriginalResponseAsync();
                    return;
                }

                if (DateTimeValidations.ChildPresenceForOneDay(dateFrom, dateTo))
                {
                    if (DateTimeValidations.DateTimeDayIsWeekendDay(dateFrom))
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            MessageController.CreateInteractionResponseMessage("Das angegebene Datum ist ein Wochenende. Für diese Tage ist keine Abmeldung notwendig!", 2));

                        await Task.Delay(DeleteTimeSpan);
                        await args.Interaction.DeleteOriginalResponseAsync();
                        return;
                    }

                    var existingDeregistration = _DataStore.DeregistrationList.FirstOrDefault(x => x.ChildId == selectChildId && x.DeregistrationDay == dateFrom);

                    if (existingDeregistration == null)
                    {
                        var newDeregistration = new ChildDeregistration()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            ChildId = (int)selectChildId,
                            DeregistrationDay = dateFrom,
                            Reason = selectChild["reason"],
                            DateDeregistrationOn = DateTime.Now,
                            DeregistrationBy = currentMember.Nickname
                        };

                        await Context.ChildDeregistrations.AddAsync(newDeregistration);
                        await Context.SaveChangesAsync();
                    }
                    else
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            MessageController.CreateInteractionResponseMessage($"Für den angegebenen Zeitraum exisitert bereits eine Abmeldung für {firstName}!", 2));

                        await Task.Delay(DeleteTimeSpan);
                        await args.Interaction.DeleteOriginalResponseAsync();
                        return;
                    }
                }
                else
                {
                    var dates = new List<DateTime>();

                    if (!DateTimeValidations.PeriodIsCorrect(dateFrom, dateTo))
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            MessageController.CreateInteractionResponseMessage($"Das Enddatum kann nicht vor dem Startdatum liegen! Das Startdatum muss immer kleiner als das Enddatum sein!", 2));

                        await Task.Delay(DeleteTimeSpan);
                        await args.Interaction.DeleteOriginalResponseAsync();
                        return;
                    }

                    while (dateFrom <= dateTo)
                    {
                        if ((dateFrom.DayOfWeek != DayOfWeek.Sunday && dateFrom.DayOfWeek != DayOfWeek.Saturday) || dateFrom < DateTime.Now)
                        {
                            var existingDeregistration = _DataStore.DeregistrationList.FirstOrDefault(x => x.ChildId == selectChildId && x.DeregistrationDay == dateFrom);
                            if (existingDeregistration == null)
                            {
                                dates.Add(dateFrom);
                                dateFrom = dateFrom.AddDays(1);
                            }
                            else
                            {
                                dateFrom = dateFrom.AddDays(1);
                                continue;
                            }
                        }
                        else
                        {
                            dateFrom = dateFrom.AddDays(1);
                        }
                    }

                    if (dates.Count != 0)
                    {
                        foreach (var date in dates)
                        {
                            var newDeregistration = new ChildDeregistration()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                ChildId = (int)selectChildId,
                                DeregistrationDay = date,
                                Reason = selectChild["reason"],
                                DateDeregistrationOn = DateTime.Now,
                                DeregistrationBy = currentMember.Nickname
                            };

                            await Context.ChildDeregistrations.AddAsync(newDeregistration);
                            await Context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            MessageController.CreateInteractionResponseMessage($"Für den angegebenen Zeitraum exisitert bereits eine Abmeldung für {firstName}!", 2));

                        await Task.Delay(DeleteTimeSpan);
                        await args.Interaction.DeleteOriginalResponseAsync();
                        return;
                    }
                }
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"Ich habe {firstName} erfolgreich abgemeldet!", 1));

                await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {firstName} {lastName} wurde erfolgreich durch den Nutzer {args.Interaction.User.Username} abgemeldet");

                await _DataStore.ReloadListOfDeregistrations();
                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {

            }
        }

        public static async Task FastDeregistrateChildToDbHandler(ComponentInteractionCreateEventArgs args)
        {
            DiscordMember currentMember = SupportController.GetCurrentUser(args.Interaction.User);
            int.TryParse(args.Values[0], out int childId);
            var firstName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == childId).FirstName;
            var lastName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == childId).LastName;

            var existingDeregistration = _DataStore.DeregistrationList.FirstOrDefault(x => x.ChildId == childId && x.DeregistrationDay == DateTime.Now.Date);

            if (DateTimeValidations.DateTimeDayIsWeekendDay(DateTime.Now.Date))
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage("Für das Wochenende sind keine Abmeldungen nötig!", 2));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
                return;
            }

            if (existingDeregistration == null)
            {
                var newDeregistration = new ChildDeregistration()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    ChildId = childId,
                    DeregistrationDay = DateTime.Now.Date,
                    Reason = "Schnellabmeldung",
                    DateDeregistrationOn = DateTime.Now,
                    DeregistrationBy = currentMember.Nickname
                };

                await Context.ChildDeregistrations.AddAsync(newDeregistration);
                await Context.SaveChangesAsync();
            }
            else
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"Für den angegebenen Zeitraum exisitert bereits eine Abmeldung für {firstName}!", 2));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
                return;
            }
            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                MessageController.CreateInteractionResponseMessage($"Ich habe {firstName} erfolgreich abgemeldet!", 1));

            await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {firstName} {lastName} wurde erfolgreich durch den Nutzer {args.Interaction.User.Username} abgemeldet");

            await _DataStore.ReloadListOfDeregistrations();
            await Task.Delay(DeleteTimeSpan);
            await args.Interaction.DeleteOriginalResponseAsync();
        }

        public static async Task RegistrateChildToDbHandler(ModalSubmitEventArgs args)
        {
            DiscordMember currentMember = SupportController.GetCurrentUser(args.Interaction.User);
            var selectChild = args.Values;
            var selectChildId = _DataStore.ChildId;
            var firstName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == selectChildId).FirstName;
            var lastName = _DataStore.ListOfChildren.FirstOrDefault(x => x.Id == selectChildId).LastName;
            DateTime dateFrom = SupportController.ParseStringToDateTime(selectChild["dateFrom"]);
            DateTime dateTo = SupportController.ParseStringToDateTime(selectChild["dateTo"]);

            if (DateTimeValidations.ChildPresenceForOneDay(dateFrom, dateTo))
            {
                var existingDeregistration = _DataStore.DeregistrationList.FirstOrDefault(x => x.ChildId == selectChildId && x.DeregistrationDay == dateFrom);

                if (existingDeregistration != null)
                {
                    var newRegistration = new ChildWithdrawnDeregistration()
                    {
                        FirstName = existingDeregistration.FirstName,
                        LastName = existingDeregistration.LastName,
                        ChildId = existingDeregistration.ChildId,
                        DeregistrationDay = existingDeregistration.DeregistrationDay,
                        Reason = existingDeregistration.Reason,
                        DateDeregistrationOn = existingDeregistration.DateDeregistrationOn,
                        DateWithdrawnDeregistration = DateTime.Now,
                        DeregistrationBy = currentMember.Nickname
                    };

                    await Context.ChildWithdrawnDeregistrations.AddAsync(newRegistration);
                    Context.ChildDeregistrations.Remove(existingDeregistration);
                    await Context.SaveChangesAsync();
                }
                else
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        MessageController.CreateInteractionResponseMessage($"Für den angegebenen Zeitraum exisitert keine Abmeldung für {firstName}!", 2));

                    await Task.Delay(DeleteTimeSpan);
                    await args.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
            }
            else
            {
                var deregistrationForWithdrawn = new List<ChildDeregistration>();

                if (!DateTimeValidations.PeriodIsCorrect(dateFrom, dateTo))
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        MessageController.CreateInteractionResponseMessage($"Das Enddatum kann nicht vor dem Startdatum liegen! Das Startdatum muss immer kleiner als das Enddatum sein!", 2));

                    await Task.Delay(DeleteTimeSpan);
                    await args.Interaction.DeleteOriginalResponseAsync();
                    return;
                }

                while (dateFrom <= dateTo)
                {
                    var existingDeregistration = _DataStore.DeregistrationList.FirstOrDefault(x => x.ChildId == selectChildId && x.DeregistrationDay == dateFrom);
                    if (existingDeregistration == null)
                    {
                        dateFrom = dateFrom.AddDays(1);
                        continue;
                    }
                    else
                    {
                        deregistrationForWithdrawn.Add(existingDeregistration);
                        dateFrom = dateFrom.AddDays(1);
                    }
                }

                if (deregistrationForWithdrawn.Count != 0)
                {
                    foreach (var date in deregistrationForWithdrawn)
                    {
                        var selectedDeregistrationInDb = Context.ChildDeregistrations.FirstOrDefault(d => d.Id == date.Id);
                        var newRegistration = new ChildWithdrawnDeregistration()
                        {
                            FirstName = date.FirstName,
                            LastName = date.LastName,
                            ChildId = date.ChildId,
                            DeregistrationDay = date.DeregistrationDay,
                            Reason = date.Reason,
                            DateDeregistrationOn = date.DateDeregistrationOn,
                            DateWithdrawnDeregistration = DateTime.Now,
                            DeregistrationBy = currentMember.Nickname
                        };

                        await Context.ChildWithdrawnDeregistrations.AddAsync(newRegistration);
                        Context.ChildDeregistrations.Remove(selectedDeregistrationInDb);
                        await Context.SaveChangesAsync();
                    }
                }
                else
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        MessageController.CreateInteractionResponseMessage($"Für den angegebenen Zeitraum exisitert keine Abmeldung für {firstName}!", 2));

                    await Task.Delay(DeleteTimeSpan);
                    await args.Interaction.DeleteOriginalResponseAsync();
                    return;
                }
            }
            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                MessageController.CreateInteractionResponseMessage($"Ich habe {firstName} erfolgreich angemeldet!", 1));

            await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {firstName} {lastName} wurde erfolgreich durch den Nutzer {args.Interaction.User.Username} angemeldet");

            await _DataStore.ReloadListOfDeregistrations();
            await Task.Delay(DeleteTimeSpan);
            await args.Interaction.DeleteOriginalResponseAsync();
        }
    }
}
