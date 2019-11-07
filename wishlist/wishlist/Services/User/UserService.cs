using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using wishlist;
using wishlist.Models.Identity;
using wishlist.Models.RequestModels.Account;

namespace wishlist.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userMgr;
        private readonly SignInManager<AppUser> signInMgr;
        private readonly ApplicationDbContext applicationDbContext;

        public UserService(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr, ApplicationDbContext applicationDbContext)
        {
            this.userMgr = userMgr;
            this.signInMgr = signInMgr;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<AppUser> FindUserByNameOrEmailAsync(string nameOrEmailAddr)
        {
            return await userMgr.FindByEmailAsync(nameOrEmailAddr);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user)
        {
            var result = await userMgr.CreateAsync(user);
            if (result.Succeeded)
            {
                await userMgr.AddToRoleAsync(user, "Customer");
                await signInMgr.SignInAsync(user, isPersistent: false);
            }
            return result;
        }

        public async Task<IList<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return (await signInMgr.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public AuthenticationProperties ConfigureExternalAuthenticaticationProperties(string provider, string redirectUrl)
        {
            return signInMgr.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await signInMgr.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey)
        {
            return await signInMgr.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent: false, bypassTwoFactor: true);
        }

        public async Task Logout()
        {
            await signInMgr.SignOutAsync();
        }

        public async Task<SignInResult> RegisterExternalUserAsync(string emailAddr, ExternalLoginInfo userLoginInfo)
        {
            var user = await FindUserByNameOrEmailAsync(emailAddr);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = emailAddr,
                    Email = emailAddr
                };
                await CreateAsync(user);
            }

            await userMgr.AddLoginAsync(user, userLoginInfo);
            return await ExternalLoginSignInAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
        }

        public async Task<string> ExternalLoginCallbackAsync(string returnUrl, string remoteError)
        {
            LoginRequest loginRequest = new LoginRequest
            {
                ReturnUrl = returnUrl,
                ExternalLogins = await GetExternalAuthenticationSchemesAsync()
            };

            if (remoteError != null)
            {
                throw new InvalidOperationException($"Error from external provider: { remoteError }");
            }

            var userLoginInfo = await GetExternalLoginInfoAsync();
            if (userLoginInfo == null)
            {
                throw new InvalidOperationException("Error loading external login information.");
            }

            var signInResult = await ExternalLoginSignInAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

            if (signInResult.Succeeded)
            {
                return returnUrl;
            }
            else
            {
                var email = userLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value;

                if (email != null)
                {
                    await RegisterExternalUserAsync(email, userLoginInfo);
                    return returnUrl;
                }
                else
                {
                    throw new InvalidOperationException($"Email claim not received from: {userLoginInfo.LoginProvider}");
                }
            }
        }

        public async Task<LoginRequest> CreateLoginRequest(string returnUrl)
        {
            LoginRequest loginRequest = new LoginRequest
            {
                ReturnUrl = returnUrl,
                ExternalLogins = await GetExternalAuthenticationSchemesAsync()
            };
            return loginRequest;
        }
    }
}
