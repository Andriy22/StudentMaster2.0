using backend.BLL.Common.VMs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IRazorRenderService
    {
        public Task<string> RenderEmailConfirmationAsync(ConfirmCodeVM model);
    }
}
