﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Company.Domain;
using Company.Domain.Exceptions;

namespace Company
{
    public static class PrincipalExtension
    {
        public static PrincipalInfo GetInfo(this ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                throw new NotAuthorizedException("Principal is not authenticated");
            }
            var identity = (ClaimsIdentity)principal.Identity;
            var claims = identity.Claims;

            PrincipalInfo result = new PrincipalInfo();
            Set(() => result.Login, claims, "login");
            return result;
        }

        private static void Set<T>(Expression<Func<T>> toSet, IEnumerable<Claim> claims, string claimName)
        {
            var claim = claims.FirstOrDefault(x => x.Type == claimName);
            if (claim != null)
            {
                Type t = toSet.Type;
                Expression assignExpr = Expression.Assign(toSet, Expression.Constant(Convert.ChangeType(claim.Value, t)));
                Expression.Lambda<Func<T>>(assignExpr).Compile()();
            }
        }
    }
}