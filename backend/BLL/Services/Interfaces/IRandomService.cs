using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IRandomService
    {
        string GetRandomPassword();
        int GetRandomNumber(int min, int max);
        string GetRandomString(int size, bool lowerCase);
    }
}
