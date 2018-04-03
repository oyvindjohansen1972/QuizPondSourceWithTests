using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizPond.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuizPond.Infrastructure
{
    public class UserManagerWrapper : IUserManagerWrapper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerWrapper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetUserIdFromIdentity(ClaimsPrincipal CurrentUser)
        {
            return _userManager.GetUserId(CurrentUser);
        }

    }

}
