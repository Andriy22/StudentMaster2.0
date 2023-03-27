namespace backend.BLL.Common.DTOs.Account;

public class ChangePasswordDTO
{
    public string Password { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}