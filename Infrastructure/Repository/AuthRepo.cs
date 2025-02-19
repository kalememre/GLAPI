using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Context;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public interface IAuthRepo
    {
        Task AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
    public class AuthRepo : IAuthRepo
    {
        private readonly GLContext _context;
        public AuthRepo(GLContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return user;
        }
    }
}