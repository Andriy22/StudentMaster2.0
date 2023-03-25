using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Admin
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string LastOnlineDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Email { get; set; }

    }
}
