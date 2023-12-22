using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
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

            switch (modalId)
            {
                case "addChildToDb":
                    {
                        await DatabaseAccessController.AddChildToDb(args);
                        break;
                    }
            }
        }
    }
}
