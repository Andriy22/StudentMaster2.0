namespace backend.BLL.Services.Interfaces;

public interface IRandomService
{
    string GetRandomPassword();
    int GetRandomNumber(int min, int max);
    string GetRandomString(int size, bool lowerCase);
}