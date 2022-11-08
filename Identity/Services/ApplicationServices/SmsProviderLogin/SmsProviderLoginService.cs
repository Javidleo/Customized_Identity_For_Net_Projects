using Identity.Common;
using Identity.Common.Exceptions;
using Identity.Models;
using Identity.Services.ApplicationServices.Authentication;
using Identity.Services.ApplicationServices.Role;
using Identity.Services.ApplicationServices.TokenStore;
using Identity.Services.ApplicationServices.User;
using Identity.Services.IdentityServices.SignInManagement;
using Identity.Services.IdentityServices.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.ApplicationServices.SmsProviderLogin
{
    public class SmsProviderLoginService : ISmsProviderLoginService
    {
        private readonly IAppUserManager _userManager;
        private readonly IUserService _userService;
        private readonly IAppSignInManager _signInManager;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;
        public SmsProviderLoginService(IAppUserManager appUserManager, IUserService userService,
                            IAppSignInManager signInManager, ITokenStoreService tokenStoreService,
                            IAuthenticationService authenticationService, IRoleService roleService)
        {
            _userManager = appUserManager;
            _userService = userService;
            _signInManager = signInManager;
            _tokenStoreService = tokenStoreService;
            _authenticationService = authenticationService;
            _roleService = roleService;
        }
        // ///////////////  result values are => identityResult/ Code  /  IsRegistration
        public async Task<(IdentityResult?, string?, bool)> Handle(string phoneNumber)
        {
            var user = await _userManager.FindByPhoneNumberAsync(phoneNumber, true);

            if (user is null) // if a verified user doesnt exist we going to create one!
            {
                var registrationResult = await Register(phoneNumber);
                if (registrationResult.Item1.Succeeded)
                    return (registrationResult.Item1, registrationResult.Item2, true);// bool value gonna be true 


                var errorMessage = IdentityErrorHandler.MergeErrorMessages(registrationResult.Item1.Errors);
                throw new NotAcceptableException(errorMessage);
                // we registering users
            }
            // else we generate a verification code and then try to login 

            var code = await _userService.VerificationTokenGeneratorAsync(user, phoneNumber);
            return (null, code, false); // is registration is false
        }

        private async Task<(IdentityResult, string)> Register(string phoneNumber)
        {
            // Register Method string result is optional : if the Registration is successfull its the code
            //                                             else it is the error message 
            var user = await _userManager.FindByPhoneNumberAsync(phoneNumber, false);
            if (user is null)
            {
                user = AppUser.Create(phoneNumber, phoneNumber); // in this case we got phoneNumber as UserName
                var result = await _userManager.CreateAsync(user);

                if (result.Errors.Any())
                    return (result, "Some Error While Creating User");

                // Default Role Assignment
                var userRoleAssignment = await _roleService.AddUserRoleAsync("Customer", user.Id, DateTime.Now);

                var token = await _userService.VerificationTokenGeneratorAsync(user, phoneNumber);
                // SmsService.Send(phoneNumber, token); need an SMS Service
                return (result, token);

            }
            else
            {
                var token = await _userService.VerificationTokenGeneratorAsync(user, phoneNumber);

                //SmsService.Send(phoneNumber, token);
                return (IdentityResult.Success, token);
            }
        }
        public async Task<JWTTokenData> Login(string phoneNumber, string code)
        {
            var result = await _userService.VerifyPhoneNumberAsync(phoneNumber, code);//return a bool value and a user instance
            if (result.Item1 is false) // item 1 is a bool flag that tell us code verified or not
                throw new NotAcceptableException("invalid verification Code");

            var user = result.Item2 as AppUser; // if code is not verified successfully user is gona be null
                                                // otherwise user is not null 

            var signInResult = await _signInManager.SmsVerificationSignIn(user.Id);
            if (signInResult.Succeeded)
            {
                await _tokenStoreService.RevokeUserTokens(user.Guid, null);

                var token = await _authenticationService.GenerateToken(user);
                if (token is null)
                    throw new NotAcceptableException("token is null");

                return token;
            }

            if (signInResult.IsLockedOut)
                throw new ForbiddenException("User Account Locked");

            if (signInResult.IsNotAllowed)
                throw new ForbiddenException("user not allow to login");

            if (signInResult.RequiresTwoFactor)
                throw new ForbiddenException("user enabled two factor its not implemented yet");

            return null;
        }
    }
}
