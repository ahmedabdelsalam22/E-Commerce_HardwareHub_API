using HardwareHub.Models.Dtos;
using HardwareHub.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
