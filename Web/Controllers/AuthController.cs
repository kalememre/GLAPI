using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Services;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">User already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] AuthRequest authRequest)
        {
            var response = await _authService.Register(authRequest);
            return Ok(response);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        /// <response code="200">User logged in successfully</response>
        /// <response code="400">Invalid credentials</response>
        /// <response code="500">Internal server error</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] AuthRequest authRequest)
        {
            var response = await _authService.Login(authRequest);
            return Ok(response);
        }
    }
}