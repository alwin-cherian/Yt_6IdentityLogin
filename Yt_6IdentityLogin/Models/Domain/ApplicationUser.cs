using Microsoft.AspNetCore.Identity;

namespace Yt_6IdentityLogin.Models.Domain
{
    public class ApplicationUser : IdentityUser 
    {
        public string Name { get; set; }

        public string ? ProfilePicture { get; set; }
    }
}
