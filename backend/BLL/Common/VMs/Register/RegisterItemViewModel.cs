using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Register
{
    public class RegisterItemViewModel
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public int Limit { get; set; }
        public bool Editable { get; set; }
    }
}
