using AutoMapper;
using HardwareHub.Models.Dtos;
using HardwareHub.Models.Models;
using HardwareHub.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HardwareHub.Data.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions _jwtOptions;

        public AuthService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IMapper mapper, IOptions<JwtOptions> jwtOptions)
        {
            _dbContext = dbContext;
            this._roleManager = roleManager;
            this._userManager = userManager;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

            List<Claim> claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.Name!),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id!),
            };

            claimList.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x))); // x meaning role

            var tokenDescripter = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            ApplicationUser? user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName!.ToLower() == model.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, model.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    applicationUserDto = null,
                    Token = null
                };
            }
            // there user is valid and exists in db .. so we will generate token.

            var roles = await _userManager.GetRolesAsync(user);

            var token = this.GenerateToken(user, roles); // responsible for generating token

            ApplicationUserDto appUserDto = _mapper.Map<ApplicationUserDto>(user);

            return new LoginResponseDTO()
            {
                applicationUserDto = appUserDto,
                Token = token
            };
        }
    }
}
