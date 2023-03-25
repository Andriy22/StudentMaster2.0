using backend.BLL.Common.VMs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<string>> GetRolesAsync();
        Task<List<UserVM>> GetUsersByRoleAsync(string role); 
    }
}
