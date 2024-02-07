using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Modals
{
    internal class ChildDeregistration
    {
        public int Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ChildId { get; set; }
        public DateTime DeregistrationDay { get; set; }
        public string Reason { get; set; }
        public DateTime DateDeregistrationOn { get; set; }
        public string? DeregistrationBy { get; set; }
        public int ChildrenGroup {  get; set; }
    }
}
