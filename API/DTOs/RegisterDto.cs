using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required] //indica il parametro obligatorio
        [StringLength(8, MinimumLength = 4)] //indica la linghezza massiama e minima della stinga
        public string Password { get; set; }
    }
}