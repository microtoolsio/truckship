using System;
using System.Threading.Tasks;
using Company.Domain;
using Company.Domain.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Company.Core
{
    public class SecurityService
    {
        private readonly CompanyUserService companyUserService;

        public SecurityService(CompanyUserService companyUserService)
        {
            this.companyUserService = companyUserService;
        }

        public async Task<ExecutionResult<CompanyUserEntity>> SignInUserToCompany(string userIdentifier, string companyIdentifier, string password)
        {
            var companyUser = await companyUserService.GetCompanyUser(userIdentifier, companyIdentifier);
            if (companyUser == null)
            {
                throw new EntityNotFoundException("No company user");
            }

            var hash = GetHashString(password, Convert.FromBase64String(companyUser.Value.Salt));
            if (hash != companyUser.Value.PasswordHash)
            {
                return new ExecutionResult<CompanyUserEntity>(ErrorInfoFacory.CreateOne("403", "Password incorrect"));
            }

            return companyUser;
        }

        private string GetHashString(string pass, byte[] salt)
        {
            var h = KeyDerivation.Pbkdf2(password: pass, salt: salt, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, numBytesRequested: 128);

            return Convert.ToBase64String(h);
        }
    }
}
