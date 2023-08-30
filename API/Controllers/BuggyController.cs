using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            /* try
            {
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Errore del Serve");
            } */
            //per non usare il try catch in ogni metodo di questa classe e in generale pe gestire gli errori creiamo un middlewear e togliamo da qui il try catch
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}