using Balu_Ass_2.Controllers;
using Balu_Ass_2.Modals;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;

namespace Balu_Ass_2.Handler
{
    internal class ChildrenTableHandler
    {
        private static readonly int DeleteTimeSpan = ProvidedSetups.BotConfig.GlobalSettings.DeleteTimeSpan;
        private static readonly ApplicationDbContext Context = ProvidedSetups.Context;
        private static readonly DiscordClient Client = ProvidedSetups.Client;
        private static int groupId;

        public static async Task AddChildToDbHandler(ModalSubmitEventArgs args)
        {
            try
            {
                if(args.Values["group"] == "1" || args.Values["group"] == "2")
                {
                    int.TryParse(args.Values["group"], out groupId);
                }
                else
                {
                    await LogController.SaveLogMessage(1, 3, $"Ungültige GroupId: {args.Values["group"]}");
                    throw new Exception();
                }

                var newChild = new Children
                {
                    FirstName = args.Values["firstName"],
                    LastName = args.Values["lastName"],
                    Mother = args.Values["nameOfMother"],
                    Father = args.Values["nameOfFather"],
                    Group = groupId,
                    DateOfLogged = DateTime.Now
                };

                await Context.Childrens.AddAsync(newChild);
                await Context.SaveChangesAsync();

                await _DataStore.ReloadListOfChildren();

                await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {newChild.FirstName} {newChild.LastName} wurde der Datenbanktabelle Childrens durch den Nutzer {args.Interaction.User.Username} hinzugefügt");
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"Ich habe {newChild.FirstName} {newChild.LastName} erfolgreich in die Datenbank eingetragen.", 1));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await LogController.SaveLogMessage(1, 3, $"Es wurde versucht ein Kind der Tabelle Childrens hinzuzufügen. Dabei trat ein Fehler in der AddChildToDb auf");
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"Leider ist ein Fehler aufgetrete. Versuche es später bitte erneut!", 3));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
        }

        public static async Task DeleteChildFromDbHandler(ComponentInteractionCreateEventArgs args)
        {
            try
            {
                var selectedChild = args.Values.FirstOrDefault();
                int.TryParse(selectedChild, out var selectedChildId);

                var childInDb = Context.Childrens.SingleOrDefault(x => x.Id == selectedChildId);
                var activeDeregistrations = _DataStore.DeregistrationList.Where(x => x.ChildId == childInDb.Id).ToList();

                if (activeDeregistrations.Count != 0)
                {
                    foreach (var deregistration in activeDeregistrations)
                    {
                        Context.ChildDeregistrations.Remove(deregistration);
                    }

                    Context.SaveChanges();

                    await _DataStore.ReloadListOfDeregistrations();
                }

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

                await LogController.SaveLogMessage(2, 1, $"Das Kind mit dem Namen {childInDb.FirstName} {childInDb.LastName} wurde durch den Nutzer {args.Interaction.User.Username} entfernt! Dabei wurden {activeDeregistrations.Count} existierende zukünftige Abmeldungen gelöscht");
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"{childInDb.FirstName} {childInDb.LastName} wurde erfolgreich aus der Datenbank entfernt!", 1));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await LogController.SaveLogMessage(1, 3, $"Bei dem Versuch ein Kind zu entfernen, ist ein Fehler aufgetreten. DatabaseAccessController.DeleteChildFromDb");
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    MessageController.CreateInteractionResponseMessage($"Leider trat ein Fehler bei dem entfernen des Kindes aus der Datenbank auf. Bitte versuche es erneut!", 3));

                await Task.Delay(DeleteTimeSpan);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
        }
    }
}
