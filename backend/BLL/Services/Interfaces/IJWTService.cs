using backend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IJWTService
    {
        string CreateRefreshToken(User user);
        string CreateToken(User user);
    }
}
