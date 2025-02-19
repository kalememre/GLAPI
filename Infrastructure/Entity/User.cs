using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Core;
using Infrastructure.LoginSecurity;

namespace Infrastructure.Entity
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public byte[]? PasswordHash { get; private set; }
        public byte[]? PasswordSalt { get; private set; }

        public void PasswordHasher(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            HashingHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            return HashingHelper.VerifyPasswordHash(password, PasswordHash!, PasswordSalt!);
        }
    }
}