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
    internal class ClientReadyController
    {
        public async Task ClientReady(DiscordClient client, ReadyEventArgs args)
        {
            await ExclusiveCommandMainView.SendExclusiveMainView();
            await ChildPresenceCommandMainView.SendChildPresenceMainView();


        }
    }
}
