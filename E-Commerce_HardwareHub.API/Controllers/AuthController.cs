using HardwareHub.Data.Services.AuthServices;
using HardwareHub.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_HardwareHub.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO loginResponse = await _service.Login(loginRequestDTO);
            if (loginResponse.applicationUserDto == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }
    }
}
