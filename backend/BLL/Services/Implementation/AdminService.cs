using backend.BLL.Common.VMs.Admin;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepository;

        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IRepository<User> userRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            return (await _roleManager.Roles.ToListAsync()).Select(x=>x.Name).ToList();
        }

        public async Task<List<UserVM>> GetUsersByRoleAsync(string role = "Admin")
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            var result = new List<UserVM>();

            foreach (var item in users)
            {
                result.Add(new UserVM
                {
                    FullName = $"{item.FirstName} {item.Name} {item.LastName}",
                    IsDeleted = item.IsDeleted,
                    Id = item.Id,
                    LastOnlineDate = item?.LastOnline.ToShortDateString(),
                    Roles = (await _userManager.GetRolesAsync(item)).ToList(),
                    Email = item.Email,
                });
            }

            return result;
        }
    }
}
