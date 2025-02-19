using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(AuthRequest request);
        Task<AuthResponse> Login(AuthRequest request);
    }
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(IAuthRepo authRepo, IMapper mapper, IConfiguration configuration)
        {
            _authRepo = authRepo ?? throw new ArgumentNullException(nameof(authRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email)
            };

            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:Expires")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _authRepo.GetUserByEmail(request.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!user.VerifyPassword(request.Password))
            {
                throw new Exception("Invalid credentials");
            }
            return new AuthResponse
            {
                Token = GenerateJwtToken(user)
            };
        }

        public async Task<AuthResponse> Register(AuthRequest request)
        {
            var existingUser = await _authRepo.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHasher(request.Password);
            await _authRepo.AddUser(user);

            return new AuthResponse
            {
                Token = GenerateJwtToken(user)
            };
        }
    }
}