using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")] // api/users
    //siccome abbiamo creato una classe per le api con tutte 
    //le attività di base da fare i controller no esstenderannò più ControllerBase
    //ma BaseApiController appena creato
    
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            //this._context = context;
            //il this si può omettere
            _context = context;
        }

        //SINCONO
        /* [HttpGet]
        //qui mi faccio ritornare tutti i record della tabella users
        //Ienumerable è una specie di lista
        public ActionResult <IEnumerable<AppUser>> GetUsers(){
            return _context.Users.ToList();
        } */

        //ASINCRONO
        /* [HttpGet("{id}")] // api/users/3
        //è uguale a quello di sopra ma qui specifico un parametro che sarà ID 
        //dell'utente che voglio ottenere, quindi il singolo utente
        public ActionResult<AppUser> GetUser(int id)
        {
            return _context.Users.Find(id);
        } */

        //ASINCRONO
        [AllowAnonymous] //questi sono middleware
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        { 
            return await _context.Users.ToListAsync();
        }

        //ASINCRONO
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id){
            return await _context.Users.FindAsync(id);
        }

    }
}