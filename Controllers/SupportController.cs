using Balu_Ass_2.Modals;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static DateTime ParseStringToDateTime(string stringDateTime)
        {
            DateTime finalDateTime = DateTime.MaxValue;
            string[] stringFormat = ["d.M.yy", "dd.MM.yy", "d.M.yyyy", "dd.MM.yyyy"];
            bool parseSuccessfull = false;

            foreach (var format in stringFormat)
            {
                if(DateTime.TryParseExact(stringDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out finalDateTime))
                {
                    parseSuccessfull = true;
                    break;
                }
            }
            if (!parseSuccessfull)
            {
                finalDateTime = DateTime.MinValue;
            }

            return finalDateTime.Date;
        }

        public static async Task<DiscordMember> GetCurrentUser(DiscordUser user)
        {
            return (DiscordMember)user;
        }
    }
}
