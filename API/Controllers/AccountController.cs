using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //  [EnableCors("MyCorsPolicy")]
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registreDto){

            if(await UserExists(registreDto.Username)) return BadRequest("Username is taken!");
            using var hmac = new HMACSHA512();
            var appuser = new AppUser{
                UserName = registreDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registreDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.users.Add(appuser);
            await _context.SaveChangesAsync();
            var token = _tokenService.CreateToken(appuser);
            return new UserDto{ Username=appuser.UserName,Token=token};
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _context.users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower() );
            if(user == null) return Unauthorized("Invalid Username");
            using var hash = new HMACSHA512(user.PasswordSalt);
            var loginHash = hash.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<loginHash.Length;i++){
                if(loginHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            
            var token = _tokenService.CreateToken(user);
            return new UserDto{ Username=user.UserName,Token=token};;
        }
        private async Task<bool> UserExists(string username){
            return await _context.users.AnyAsync(x=> x.UserName == username.ToLower());
        }
    }
}