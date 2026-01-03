using CoreBanking.Application.Common.DTOs;
using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Features.Auths.Commands
{
    // Command để đăng nhập và nhận về JWT token
    public record LoginCommand(LoginDTO login): IRequest<string>;
    // Handler cho LoginCommand
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public LoginCommandHandler(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            // Kiểm tra thông tin đăng nhập (giả lập)
            var user = _context.Users.FirstOrDefault(c => c.FullName==request.login.Username);
            if (user==null) {
                throw new Exception("Thông tin đăng nhập không hợp lệ.");
            }
            //Kiểm tra mật khẩu (giả lập)
            if(!BCrypt.Net.BCrypt.Verify(request.login.Password, user.PasswordHash))
            {
                throw new Exception("Thông tin đăng nhập không hợp lệ.");
            }

            // Tạo JWT token
            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }

}
