using API.Entities;
using API.Extensions;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; } //foto principale dell'utente
        public int Age { get; set; }
        public string KnowAs { get; private set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<PhotoDto> Photos { get; set; } = new();//ogni utente può avere più di una foto
    }


}