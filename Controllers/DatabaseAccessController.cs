using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Modals;
using Balu_Ass_2.Validations;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class DatabaseAccessController
    {
        private static readonly int DeleteTimeSpan = ProvidedSetups.BotConfig.GlobalSettings.DeleteTimeSpan;
        private static readonly ApplicationDbContext Context = ProvidedSetups.Context;

        public static async Task AddChildToDb(ModalSubmitEventArgs args)
        {
            try
            {
                var newChild = new Children
                {
                    FirstName = args.Values["firstName"],
                    LastName = args.Values["lastName"],
                    Mother = args.Values["nameOfMother"],
                    Father = args.Values["nameOfFather"],
                    DateOfLogged = DateTime.Now
                };

                await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {newChild.FirstName} {newChild.LastName} wurde der Datenbanktabelle Childrens durch den Nutzer {args.Interaction.User.Username} hinzugefügt");

                await Context.Childrens.AddAsync(newChild);
                await Context.SaveChangesAsync();

                await _DataStore.ReloadListOfChildren();

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Ich habe {newChild.FirstName} {newChild.LastName} erfolgreich in die Datenbank eingetragen."));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Leider trat ein Fehler in der Datenbankverbindung auf. Bitte versuche es erneut!"));

                await LogController.SaveLogMessage(1, 3, $"Es wurde versucht ein Kind der Tabelle Childrens hinzuzufügen. Dabei trat ein Fehler in der AddChildToDb auf");

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
        }

        public static async Task DeleteChildFromDb(ComponentInteractionCreateEventArgs args)
        {
            try
            {
                var selectedChild = args.Values.FirstOrDefault();
                int.TryParse(selectedChild, out var selectedChildId);

                var childInDb = Context.Childrens.SingleOrDefault(x => x.Id == selectedChildId);

                var deletedChildren = new DeletedChildren
                {
                    FirstName = childInDb.FirstName,
                    LastName = childInDb.LastName,
                    Mother = childInDb.Mother,
                    Father = childInDb.Father,
                    DateOfLogged = childInDb.DateOfLogged,
                    DateOfDeletion = DateTime.Now
                };

                await Context.DeletedChildrens.AddAsync(deletedChildren);
                Context.Childrens.Remove(childInDb);
                await Context.SaveChangesAsync();

                await _DataStore.ReloadListOfChildren();

                await LogController.SaveLogMessage(2, 1, $"Das Kind mit dem Namen {childInDb.FirstName} {childInDb.LastName} wurde durch den Nutzer {args.Interaction.User.Username} entfernt!");

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"{childInDb.FirstName} {childInDb.LastName} wurde erfolgreich entfernt"));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Leider trat ein Fehler bei dem entfernen des Kindes aus der Datenbank auf. Bitte versuche es erneut!"));

                await LogController.SaveLogMessage(1, 3, $"Bei dem Versuch ein Kind zu entfernen, ist ein Fehler aufgetreten. DatabaseAccessController.DeleteChildFromDb");

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
        }

        public static async Task DeregistrateChild(ModalSubmitEventArgs args)
        {
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
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkRed).WithTitle("Fehler bei der Abmeldung").WithDescription("Das angegebene Datum liegt in der Vergangenheit!")));

                    await Task.Delay(DeleteTimeSpan);
                    await args.Interaction.DeleteOriginalResponseAsync();
                    return;
                }

                if (DateTimeValidations.ChildPresenceForOneDay(dateFrom, dateTo))
                {
                    if (DateTimeValidations.DateTimeDayIsWeekendDay(dateFrom))
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.Yellow).WithTitle("Abmeldung für ein Wochenende").WithDescription("Das angegebene Datum ist ein Wochenende. Für diese Tage ist keine Abmeldung notwendig!")));

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
                            DateDeregistrationOn = DateTime.Now
                        };

                        await Context.ChildDeregistrations.AddAsync(newDeregistration);
                        await Context.SaveChangesAsync();

                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkGreen).WithTitle("Abmeldung erfolgreich").WithDescription($"Ich habe {firstName} erfolgreich abgemeldet!")));
                    }
                    else
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.Yellow).WithTitle("Abmeldung existiert bereits").WithDescription($"Für den angegebenen Zeitraum exisitert bereits eine Abmeldung für {firstName}!")));
                    }
                }
                else
                {
                    var dates = new List<DateTime>();

                    if (!DateTimeValidations.PeriodIsCorrect(dateFrom, dateTo))
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkRed).WithTitle("Der angegebene Zeitraum ist fehlerhaft").WithDescription($"Das Enddatum kann nicht vor dem Startdatum liegen! Das Startdatum muss immer kleiner als das Enddatum sein!")));

                        await Task.Delay(DeleteTimeSpan);
                        await args.Interaction.DeleteOriginalResponseAsync();
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
                                DateDeregistrationOn = DateTime.Now
                            };

                            await Context.ChildDeregistrations.AddAsync(newDeregistration);
                            await Context.SaveChangesAsync();
                        }

                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.DarkGreen).WithTitle("Abmeldung erfolgreich").WithDescription($"Ich habe {firstName} erfolgreich abgemeldet!")));
                    }
                    else
                    {
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .AddEmbed(new DiscordEmbedBuilder().WithColor(DiscordColor.Yellow).WithTitle("Abmeldung existiert bereits").WithDescription($"Für den angegebenen Zeitraum exisitert bereits eine Abmeldung für {firstName}!")));
                    }
                }

                await _DataStore.ReloadListOfDeregistrations();
                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}
