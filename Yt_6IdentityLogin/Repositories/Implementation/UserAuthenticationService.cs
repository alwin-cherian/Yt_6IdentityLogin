using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Yt_6IdentityLogin.Models.Domain;
using Yt_6IdentityLogin.Models.DTO;
using Yt_6IdentityLogin.Repositories.Abstract;

namespace Yt_6IdentityLogin.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenticationService(SignInManager<ApplicationUser> signInManager , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                status.StatusCode = 0;
                status.Message = " Invalid Username";
                return status;
            }
            // we will match password
            if(!await userManager.CheckPasswordAsync(user , model.Password))
            {
                status.StatusCode = 0;
                status.Message = " Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if(signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                }
                status.StatusCode = 1;
                status.Message = " Logged in successfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = " User locked out ";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = " Error on logging In";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegisterationAsync(RegisterationModel model)
        {
            var status =  new Status();
            var UserExists = await userManager.FindByNameAsync(model.UserName);
            if (UserExists != null)
            {
                status.StatusCode = 0;
                status.Message = " User already exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user , model.Password);
            if(!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Creation Failed";
                return status;
            }

            //role manager
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if(await roleManager.RoleExistsAsync(model.Role))
                await userManager.AddToRoleAsync(user, model.Role);

            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;
        }
    }
}
