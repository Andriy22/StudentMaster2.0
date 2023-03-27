namespace backend.BLL.Common.VMs.Group;

public class UserShortInfoVM
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public bool IsDeleted { get; set; } = false;
}