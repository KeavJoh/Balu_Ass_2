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
                        break;
                    }
            }
        }
    }
}
