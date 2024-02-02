using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Modals
{
    internal class ChildWithdrawnDeregistration
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ChildId { get; set; }
        public DateTime DeregistrationDay { get; set; }
        public string Reason { get; set; }
        public DateTime DateDeregistrationOn { get; set; }
        public string? DeregistrationBy { get; set; }
        public DateTime DateWithdrawnDeregistration { get; set; }
        public string? WithdrawnBy { get; set; }
    }
}
