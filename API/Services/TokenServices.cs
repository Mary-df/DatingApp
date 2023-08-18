using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenServices : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenServices(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokenkey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claim = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            //creiamo le credenziali quiondi creo la chieave subito dopo il valore
            //qui sto creando la terza parte del token cioè la firma che poi verrà salvata nel server
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            
            //qui creo il token che andrà restituito
            //quindi abbiamo bisogno di un'oggetto che includerà le affermazioni che vogliamo includere
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claim),
                    //nuova richiesta di risarcimento
                Expires = DateTime.Now.AddDays(7),
                    //data di scadenza inquesto caso 7 giorni
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
                //salviamo il nuovo tojken come nuovo gestore di token 

            var token = tokenHandler.CreateToken(tokenDescriptor);
                //creiamo e salviamo il nostro token nel metodo vanno messe le 
                //nostre richieste

            return tokenHandler.WriteToken(token);
                //mi faccio ritornare il token
        }
    }
}