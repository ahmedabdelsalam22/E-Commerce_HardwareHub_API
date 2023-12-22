using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Models.Dtos
{
    public class LoginResponseDTO
    {
        public ApplicationUserDto? applicationUserDto { get; set; }
        public string? Token { get; set; }
    }
}
