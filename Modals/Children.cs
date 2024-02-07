using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balu_Ass_2.Modals
{
    internal class Children
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Mother { get; set; }
        public string? Father { get; set; }
        public int ChildrenGroup {  get; set; }
        public DateTime DateOfLogged { get; set; }
    }
}
