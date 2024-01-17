using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Controllers;
using Balu_Ass_2.Data.Database;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Modals
{
    internal class _DataStore
    {
        private static readonly ApplicationDbContext context = ProvidedSetups.Context;

        public static List<Children> ListOfChildren { get; set; }

        public static async Task InitDataStore()
        {
            await ReloadListOfChildren();
        }

        public static async Task ReloadListOfChildren()
        {
            try
            {
                ListOfChildren = await context.Childrens.OrderBy(x => x.FirstName).ToListAsync();
            }
            catch (Exception)
            {
                await LogController.SaveLogMessage(1, 3, "Es ist ein Fehler beim Laden der Liste aller Kinder aufgetreten. _DataStore.ReloadListOfChildren()");
            }
        }

        public static async Task<List<DiscordSelectComponentOption>> GetChildrensList()
        {
            var options = new List<DiscordSelectComponentOption>();

            foreach (var child in ListOfChildren)
            {
                options.Add(new DiscordSelectComponentOption(child.FirstName + " " + child.LastName, child.Id.ToString()));
            }

            return options;
        }
    }
}
