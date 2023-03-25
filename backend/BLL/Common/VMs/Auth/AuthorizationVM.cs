using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.VMs.Auth
{
    //public class AuthorizationVM
    //{
    //    public string UserId { get; set; }
    //    public string AccessToken { get; set; }
    //    public string RefreshToken { get; set; }
    //    public string UserName { get; set; }
    //    public string FullName { get; set; }
    //    public string AvatarSrc { get; set; }
    //    public List<string> Roles { get; set; }

    //    public AuthorizationVM()
    //    {
    //        Roles = new List<string>();
    //    }
    //}

    public class AuthorizationVM
    {
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
    }
}
