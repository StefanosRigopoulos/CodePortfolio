using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        public readonly ITokenService _tokenService;
        public readonly IMapper _mapper;

        public AccountController (DataContext context, ITokenService tokenService, IMapper mapper){
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO){
            if (await UsernameExists(registerDTO.Username)) return BadRequest("Username is taken");
            if (await EmailExists(registerDTO.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<AppUser>(registerDTO);

            using var hmac = new HMACSHA512();

            user.UserName = registerDTO.Username.ToLower();
            user.NickName = registerDTO.Username;
            user.Photo = new Photo();
            user.Photo.URL = registerDTO.PhotoURL;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO){

            // Checking the username
            var user = await _context.Users.Include(p => p.Photo).SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);
            if (user == null) return Unauthorized("Invalid username!");

            // Checking the password
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            for (int i = 0; i < computedHash.Length; i++){
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password!");
            }

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                ProfilePhotoURL = user.Photo?.URL
            };
        }

        private async Task<bool> UsernameExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        private async Task<bool> EmailExists(string email){
            return await _context.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
