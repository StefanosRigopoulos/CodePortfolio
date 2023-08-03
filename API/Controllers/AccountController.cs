using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        public readonly ITokenService _tokenService;
        public readonly IMapper _mapper;

        public AccountController (UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper){
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO){
            if (await UsernameExists(registerDTO.Username)) return BadRequest("Username is taken");
            if (await EmailExists(registerDTO.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();
            user.NickName = registerDTO.Username;
            user.Photo = new Photo();
            user.Photo.URL = registerDTO.PhotoURL;

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Nickname = user.NickName,
                Token = await _tokenService.CreateToken(user),
                ProfilePhotoURL = user.Photo?.URL,
                CodeLanguage = user.CodeLanguage
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO){

            // Checking the username
            var user = await _userManager.Users.Include(p => p.Photo).SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);
            if (user == null) return Unauthorized("Invalid username!");

            // Checking the password
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) return Unauthorized("Invalid Password");
            return new UserDTO
            {
                Username = user.UserName,
                Nickname = user.NickName,
                Token = await _tokenService.CreateToken(user),
                ProfilePhotoURL = user.Photo?.URL,
                CodeLanguage = user.CodeLanguage
            };
        }

        private async Task<bool> UsernameExists(string username){
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        private async Task<bool> EmailExists(string email){
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
