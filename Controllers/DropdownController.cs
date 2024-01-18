using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Views;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class DropdownController
    {
        public static async Task DropdwonSubmitEvent(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            var dropdownId = args.Interaction.Data.CustomId;
            var channelId = await client.GetChannelAsync(ProvidedSetups.BotConfig.ChannelIds.ExclusiveViewChannel);

            switch(dropdownId)
            {
                case "deleteChildFromDbDropdwon":
                    {
                        await DatabaseAccessController.DeleteChildFromDb(args);
                        await SupportController.DeleteLastMessage(channelId);
                        break;
                    }
                case "deregistrateChildDropdown":
                    {
                        await ChildPresenceCommandMainView.DeregistrateChildModal(args);
                        break;
                    }
            }
        }
    }
}
