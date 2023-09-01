using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")] 
    // Photos è il nome tabella del db
    //specifica come voglio che si chiama la tabella nell'attributo lis
    //specifica la corrispondenza tra una classe e una tabella di db
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; } //indica la foto principale dell'utente
        public string PublicId { get; set; } //documento d'identità pubblico per archiviare le foto
        //le proprietà di seguito servono per creare la relazione nel db
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}