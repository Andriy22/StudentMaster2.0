using backend.BLL.Common.VMs.Email;

namespace backend.BLL.Services.Interfaces;

public interface IRazorRenderService
{
    public Task<string> RenderEmailConfirmationAsync(ConfirmCodeVM model);
}