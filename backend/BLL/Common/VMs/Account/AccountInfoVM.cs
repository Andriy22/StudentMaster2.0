using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Account
{
    public class AccountInfoVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
    }
}
