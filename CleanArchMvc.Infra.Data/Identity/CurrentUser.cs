﻿using CleanArchMvc.Domain.Interfaces.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace CleanArchMvc.Infra.Data.Identity
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Email => GetUserClaim(ClaimTypes.Email);

        private string GetUserClaim(string claimType)
        {
            var user = _httpContextAccessor.HttpContext.User;
            return user.Claims.FirstOrDefault(c => c.Type.Equals(claimType))?.Value;
        }
    }
}