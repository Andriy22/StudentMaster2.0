using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Register
{
    public class PresetViewModel
    {
        public PresetViewModel()
        {
            Items = new List<PresetItemViewModel>();
        }
        public string Name { get; set; }
        public List<PresetItemViewModel> Items { get; set; }
    }

    public class PresetItemViewModel
    {
        public string Name { get; set; }         
        public int MaxGrade { get; set; }
        public bool Removable { get; set; }
    }
}
