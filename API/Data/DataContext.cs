using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){}
        public DbSet<AppUser> Users { get; set; }
        //non ho bisogno di creare un dbSet per le foto perchè non andrò mai ad interrogare questo db

    }
}