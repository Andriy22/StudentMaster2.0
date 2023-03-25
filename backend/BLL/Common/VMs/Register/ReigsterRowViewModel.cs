using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Register
{
    public class RegisterRowViewModel
    {
        public RegisterRowViewModel(string header, string name)
        {
            Header = header;
            Name = name;
            Items = new List<RegisterItemViewModel>();
        }

        public string Header { get; set; }
        public string Name { get; set; }

        public List<RegisterItemViewModel> Items { get; set; }
    }
}
