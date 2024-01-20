using Balu_Ass_2.BotSettings;
using Balu_Ass_2.Data.Database;
using Balu_Ass_2.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Controllers
{
    internal class TimerActionController
    {
        private static readonly ApplicationDbContext Context = ProvidedSetups.Context;

        public static async Task TimerActions()
        {
            if (DateTime.Now.Hour == 02 && DateTime.Now.Minute == 30)
            {
                await RenewChildPresence();
            }
        }

        private static async Task RenewChildPresence()
        {
            List<ChildDeregistration> expiredDeregistrations = Context.ChildDeregistrations.Where(x => x.DeregistrationDay.Date < DateTime.Now.Date).ToList();
            if (expiredDeregistrations.Count > 0)
            {
                foreach (var expiredDeregistration in expiredDeregistrations)
                {
                    var expiredObject = new ChildExpiredDeregistration
                    {
                        FirstName = expiredDeregistration.FirstName,
                        LastName = expiredDeregistration.LastName,
                        ChildId = expiredDeregistration.ChildId,
                        DeregistrationDay = expiredDeregistration.DeregistrationDay,
                        Reason = expiredDeregistration.Reason,
                        DateDeregistrationOn = expiredDeregistration.DateDeregistrationOn,
                        DeregistrationBy = expiredDeregistration.DeregistrationBy
                    };

                    await Context.ChildExpiredDeregistrations.AddAsync(expiredObject);
                    Context.ChildDeregistrations.Remove(expiredDeregistration);
                    await Context.SaveChangesAsync();
                }

                await _DataStore.ReloadListOfDeregistrations();

                await LogController.SaveLogMessage(3, 1, $"Tägliche Abmeldungsprüfung wurde um {DateTime.Now} durchgeführt");
            }
        }
    }
}
