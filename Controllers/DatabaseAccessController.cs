using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Modals;
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

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Ich habe {newChild.FirstName} {newChild.LastName} erfolgreich in die Datenbank eingetragen."));

                await Task.Delay(10000);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Leider trat ein Fehler in der Datenbankverbindung auf. Bitte versuche es erneut!"));

                await LogController.SaveLogMessage(1, 3, $"Es wurde versucht ein Kind der Tabelle Childrens hinzuzufügen. Dabei trat ein Fehler in der AddChildToDb auf");

                await Task.Delay(10000);
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

                await Task.Delay(10000);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception)
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Leider trat ein Fehler bei dem entfernen des Kindes aus der Datenbank auf. Bitte versuche es erneut!"));

                await LogController.SaveLogMessage(1, 3, $"Bei dem Versuch ein Kind zu entfernen, ist ein Fehler aufgetreten. DatabaseAccessController.DeleteChildFromDb");

                await Task.Delay(10000);
                await args.Interaction.DeleteOriginalResponseAsync();
            }
        }
    }
}
