using backend.BLL.Common.VMs.Admin;

namespace backend.BLL.Services.Interfaces;

public interface IAdminService
{
    Task<List<string>> GetRolesAsync();
    Task<List<UserVM>> GetUsersByRoleAsync(string role);
}