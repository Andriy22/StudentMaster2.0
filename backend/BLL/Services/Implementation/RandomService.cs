using System.Text;
using backend.BLL.Services.Interfaces;

namespace backend.BLL.Services.Implementation;

public class RandomService : IRandomService
{
    public string GetRandomPassword()
    {
        var builder = new StringBuilder();
        builder.Append(GetRandomString(4, true));
        builder.Append(GetRandomNumber(1000, 9999));
        builder.Append(GetRandomString(2, false));
        return builder.ToString();
    }

    public int GetRandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    public string GetRandomString(int size, bool lowerCase)
    {
        var builder = new StringBuilder();
        var random = new Random();
        char ch;
        for (var i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }

        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }
}