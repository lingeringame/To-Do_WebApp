using Microsoft.AspNetCore.Identity;

namespace To_Do.Areas.Identity.Data
{
    public class To_DoUser : IdentityUser
    {
        //Add custom data to users
        [PersonalData]
        public string Name { get; set; }
    }
}
