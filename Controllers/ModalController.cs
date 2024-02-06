using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Modals;
using Balu_Ass_2.Views;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class ModalController
    {
        public static async Task ModalSubmitEvent(DiscordClient client, ModalSubmitEventArgs args)
        {
            var modalId = args.Interaction.Data.CustomId;
            var exclusiveChannelId = await client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.ExclusiveViewChannel);
            var presenceChannelId = await client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.ChildPresenceViewChannel);

            switch (modalId)
            {
                case "addChildToDb":
                    {
                        await DatabaseAccessController.AddChildToDb(args);
                        break;
                    }
                case "deregistrateChildModal":
                    {
                        await DatabaseAccessController.DeregistrateChild(args);
                        await SupportController.DeleteLastMessage(presenceChannelId);
                        break;
                    }
                case "registrateChildModal":
                    {
                        await DatabaseAccessController.RegistrateChild(args);
                        await SupportController.DeleteLastMessage(presenceChannelId);
                        break;
                    }
                case "createVoting":
                    {
                        await VotingInformationView.CreateVotingAsync(args);
                        break;
                    }
                case "editChildToDb":
                    {
                        await DatabaseAccessController.EditChild(args);
                        await SupportController.DeleteLastMessage(exclusiveChannelId);
                        break;
                    }
            }
        }
    }
}
