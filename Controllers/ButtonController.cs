using Balu_Ass_2.Modals;
using Balu_Ass_2.Views;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class ButtonController
    {
        public static async Task ButtonClickEvent(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            var buttonId = args.Interaction.Data.CustomId;

            switch (buttonId)
            {
                case "addChildToDb":
                    {
                        await ExclusiveCommandMainView.AddChildToDbModal(args);
                        break;
                    }
                case "deleteChildFromDb":
                    {
                        await ExclusiveCommandMainView.DeleteChildFromDbDropdown(args);
                        break;
                    }
                case "deregistrateChild":
                    {
                        await ChildPresenceCommandMainView.DeregistrateChildDropdown(args);
                        break;
                    }
                case "fastDeregistrateChild":
                    {
                        await ChildPresenceCommandMainView.FastDeregistrateChildDropdown(args);
                        break;
                    }
                case "registrateChild":
                    {
                        await ChildPresenceCommandMainView.RegistrateChildDropdown(args);
                        break;
                    }
                case "createVoting":
                    {
                        await VotingInformationView.CreateVotingModal(args);
                        break;
                    }
                case "editChild":
                    {
                        await ExclusiveCommandMainView.EditChildDropdown(args);
                        break;
                    }
            }
        }
    }
}
