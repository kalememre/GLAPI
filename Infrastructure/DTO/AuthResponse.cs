using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class AuthResponse
    {
        public required string Token { get; set; }
    }
}