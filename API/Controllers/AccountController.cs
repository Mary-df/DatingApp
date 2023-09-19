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
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(username: registerDto.Username)) return BadRequest("Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDto); //così e tutto uguale allo user
            
            using var hmac = new HMACSHA512();

            /* var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };DOPO AVER AGGIUNTO IL MAPPER DIVENTA */

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto {
                Username =  user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //vado a prendere lo user
            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            //SingleOrDefaultAsync --> cerca nel db il valore che gli viene passato e 
            //se ne trova nulla ritorna il valore di default/predefinito che solitamente è "null
            //senza creare eccezioni
            //SingleAsync --> lancia un'eccezione se il valore di ritorno è null o l'utente non 
            //esiste. Solitamente si utilizza quando si è certi che quel valore esiste nel db
            if (user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

             for (int i = 0; i < computedHash.Length; i++)
             {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
             }

             return new UserDto {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
             };
        }

        private async Task<bool> UserExists(string username)
        {
            //Task<bool> Task --> classe di c# <bool> perchè vuoi che ti torni un booleano
            //e gli passi lo usernme da controllare
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
            //X sta per la tabella chiamata dopo il context quindi users
            //quindi x. accetta tutto quello che c'è nella tabella users quindi 
            //es x.Code a patto che sia una stringa perchè gli passo una strings
            //lui non può e non vuole fare il confronto con tipi diversi
        }

    }
}