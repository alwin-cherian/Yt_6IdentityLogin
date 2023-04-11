using System.ComponentModel.DataAnnotations;

namespace Yt_6IdentityLogin.Models.DTO
{
    public class UserViewModel
    {
        //public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
