namespace backend.BLL.Common.DTOs.Account;

public class ConfirmAccountDTO
{
    public int Code { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}