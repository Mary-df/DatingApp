using System.Linq;
using System.Security.AccessControl;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper  mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            //SENZA MAPPATORE
            /*return await _context.Users
            .Where(x => x.UserName == username)
            .Select(user => new MemberDto {
                Id = user.Id,
                City = user.City,
                Gender = user.Gender,
                //etc
            }).SingleOrDefaultAsync(); //questo proietterà nel meberDto quello che voglio*/

            //CON IL MAPPATORE
            //Quando usiamo il mapper non dobbiamo fare l'includ delle tabelle
            //correlate ci penserà il mappatore per noi
            return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
                    
            /*
            Find() -->  - utilizzato per cercare un elemento all'interno di una collezione o array
                        - non è parte di Linq e non è disponibile per altre collezioni
                        - restituisce il primo elemeto all'interno della lista se non disponoibile restituisce 0 o null
            First() --> - utilizzato per cercare elementi in qualsiasi collezione che implementi IEnumerable
                        - fa parte di Linq
                        - restituisce il primo elemento all'interno di una lista e se non lo trova genere un'eccezione
        */
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
            /*
            **** Include() ****
            in questo metodo senza l'inclue verranno visualizzati tutti gli utenti ma senza foto
            per far in modo di vedere le foto o qualsiasi tabella collegata bisogna aggiungere Include() 
            
            **** SingleOrDefault() ****
            è un metodo che può essere utilizzato per eseguire una query 
            su una raccolta di dati (es. database)
            restituire un singolo elemento che soddisfa una condizione specificata
            se esistono più elementi che soddisfano la condizione genera un'eccezione
            usato quando ci sia aspetta al massimo un elemento
            se non trova nessun elemento ritorna null
            */
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            /* 
            Entry --> - fa farte di entity framework 
                      - ritorna un'oggetto che rappresenta l'entità
                      - fornisce metodi e proprietà per gestite e monitotare lo ststo dell'entità
                         ed aggiungere modoficare o eliminare dati dal db
            */
        }
    }
}