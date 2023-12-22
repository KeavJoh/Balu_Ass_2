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

                await Context.Childrens.AddAsync(newChild);
                await Context.SaveChangesAsync();

                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Ich habe {args.Values["firstName"]} erfolgreich in die Datenbank eingetragen."));

                await LogController.SaveLogMessage(2, 1, $"Kind mit dem Namen {args.Values["firstName"]} wurde der Datenbanktabelle Childrens durch den Nutzer {args.Interaction.User.Username} hinzugefügt");
            }
            catch (Exception ex)
            {
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent($"Leider trat ein Fehler in der Datenbankverbindung auf. Bitte versuche es erneut!"));

                await LogController.SaveLogMessage(1, 3, $"Es wurde versucht ein Kind der Tabelle Childrens hinzuzufügen. Dabei trat ein Fehler in der AddChildToDb auf");
            }

            await Task.Delay(10000);
            await args.Interaction.DeleteOriginalResponseAsync();
        }
    }
}
