using HardwareHub.Data.Services.AuthServices;
using HardwareHub.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO loginResponse = await _service.Login(loginRequestDTO);
            if (loginResponse.applicationUserDto == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApplicationUserDto>> Register([FromBody] RegisterRequestDTO model)
        {
            if (model.Name.ToLower() == model.UserName.ToLower())
            {
                ModelState.AddModelError("", "username and name are the same!");
            }
            bool ifUserNameUnique = _service.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                ModelState.AddModelError("", "Username already exists");
            }

            var userDTO = await _service.Register(model);

            if (userDTO != null)
            {
                return userDTO;
            }
            else
            {
                return new ApplicationUserDto();
            }
        }
        [HttpPost("assignRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRole(RegisterRequestDTO model, string roleName)
        {
            bool roleIsAssigned = await _service.AssignRole(model.Email, roleName!.ToUpper());
            if (!roleIsAssigned)
            {
                return BadRequest("error occured!");
            }
            return Ok("role assigned successfully");
        }

    }
}
