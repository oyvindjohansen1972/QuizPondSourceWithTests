using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizPond.Data.Models;
using QuizPond.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace QuizPond.Tests.Fakes
{
    public class FakeUserManagerRepository : IUserManagerWrapper
    {
        public string GetUserIdFromIdentity(ClaimsPrincipal CurrentUser)
        {
            return "18e8eeec-c795-42af-bd61-1c192b5c6e6c";
        }
    }
}
