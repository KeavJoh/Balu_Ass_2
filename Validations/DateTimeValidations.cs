using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Validations
{
    internal class DateTimeValidations
    {
        public static bool DateTimeInThePast(DateTime date)
        {
            if (date < DateTime.Now.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static bool ChildPresenceForOneDay(DateTime dateFrom, DateTime dateTo)
        {
            if (dateTo == DateTime.MinValue || dateTo == dateFrom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DateTimeDayIsWeekendDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool PeriodIsCorrect(DateTime start, DateTime end)
        {
            if (end < start)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
