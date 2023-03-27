using backend.DAL.Entities;

namespace backend.BLL.Services.Interfaces;

public interface IJWTService
{
    string CreateRefreshToken(User user);
    string CreateToken(User user);
}