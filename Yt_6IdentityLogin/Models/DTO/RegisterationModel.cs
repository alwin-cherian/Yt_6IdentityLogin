﻿using System.ComponentModel.DataAnnotations;

namespace Yt_6IdentityLogin.Models.DTO
{
    public class RegisterationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$" , ErrorMessage = "Minimum length 6 and must contain 1 uppercase, 1 lowercase , 1 special chararcter and 1 digit")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        public string? Role { get; set; }
    }
}
