using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            //this._context = context;
            //il this si può omettere
            //Una volta creato il repositori togliamo il context e lasciamo il repository
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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
        //[AllowAnonymous] 
        //AllowAnonymous lo tolgo perchè non voglio assolutamente che una persona non loggata
        //possa avere l'elenco degli utenti o un singolo utente
        [HttpGet] //questi sono middleware
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {   //SENZA MAPPATORE
            //con l'aggiunta del map questo va tolto e divente
            //return Ok(await _userRepository.GetUsersAsync());
            //var users = await _userRepository.GetUsersAsync();
            //var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            //vado a mappare una lista di utenti in una lista di oggetti questo
            //è utile quando si vogliono trasformare o nascondere alcune informazioni
            //prima di restituire al cliente
            //return Ok(userToReturn); //restituisco una risposta http con status 200 e la lista di utenti

            //CON IL MAPPATORE
            return Ok(await _userRepository.GetMembersAsync());
        }

        //ASINCRONO
        [HttpGet("{username}")]
        //qui cambio Task<ActionResult<AppUser>> con Task<ActionResult<MamberDto>> perchè ci sono delle info che voglio nascondere
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //togliamo la mappatura perchè lo facciamo direttamenter nel repository usando memberDto
            //var user = await  _userRepository.GetUserByUsernameAsync(username);
            //otteniamo il nome dello user e lo salviamo nella variabile user
            //return _mapper.Map<MemberDto>(user);
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            //      -> lo spostiamo in ClaimsPrincipal in getUsername
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Aggiornamento utente fallito");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if(user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);


            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction( nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo) );
            //CreatedAtAction --> ritorna uno Status 201
            //nameof(GetUser) -> nome del metodo da andare a prendere
            // new { username = user.UserName } -> il nuovo parametro da passare all'oggetto
            // _mapper.Map<PhotoDto>(photo) -> per le foto 
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("la foto scelta è già una foto principale");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return  NoContent();
            return BadRequest("Problem setting the main photo");
        }

        
    }
}