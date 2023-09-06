namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string Code { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        //DateOnly ci permette di tracciare la data di qualcosa
        public string KnowAs { get; private set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        //public List<Photo> Photos { get; set; } = new List<Photo>();
        //una volta creata la classe per le foto possiamo abbreviare
        public List<Photo> Photos { get; set; } = new();
        //proprietà dell'età che viene calcolata dalla data di nascita

        /* public int GetAge()
        {
            return DateOfBirth.CalcuateAge();
        } */
    }
}