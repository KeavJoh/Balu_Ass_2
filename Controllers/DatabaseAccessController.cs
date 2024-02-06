using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Handler;
using Balu_Ass_2.Modals;
using Balu_Ass_2.Validations;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class DatabaseAccessController
    {
        public static async Task AddChildToDb(ModalSubmitEventArgs args)
        {
            await ChildrenTableHandler.AddChildToDbHandler(args);
        }

        public static async Task DeleteChildFromDb(ComponentInteractionCreateEventArgs args)
        {
            await ChildrenTableHandler.DeleteChildFromDbHandler(args);
        }

        public static async Task DeregistrateChild(ModalSubmitEventArgs args)
        {
            await ChildDeregistrationTableHandler.DeregistrateChildToDbHandler(args);
        }

        public static async Task FastDeregistrateChild(ComponentInteractionCreateEventArgs args)
        {
            await ChildDeregistrationTableHandler.FastDeregistrateChildToDbHandler(args);
        }

        public static async Task RegistrateChild(ModalSubmitEventArgs args) 
        {
            await ChildDeregistrationTableHandler.RegistrateChildToDbHandler(args);
        }

        public static async Task EditChild(ModalSubmitEventArgs args)
        {
            await ChildrenTableHandler.EditChildFromDb(args);
        }
    }
}
