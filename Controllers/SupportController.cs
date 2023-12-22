using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class SupportController
    {
        public static async Task DeleteAllMessages(DiscordChannel channel)
        {
            var allMessages = await channel.GetMessagesAsync();
            if (allMessages.Count > 0)
            {
                await channel.DeleteMessagesAsync(allMessages);
            }
        }

        public static async Task DeleteLastMessage(DiscordChannel channel)
        {
            var message = await channel.GetMessagesAsync(1);
            var latestMessage = message.FirstOrDefault();
            if (latestMessage != null)
            {
                await latestMessage.DeleteAsync();
            }
        }
    }
}
