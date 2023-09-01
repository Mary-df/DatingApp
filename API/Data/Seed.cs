using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        //classe creata per generare dati fittizi nel db

        public static async Task SeedUsers(DataContext context)
        {
            //se il db è già ppopolato non faccio nulla
            if (await context.Users.AnyAsync()) return;
            //atrimenti lo vado a popolare con il file json che ho creato in data
            //quindi faccio la lettura del file
            var UserData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(UserData);
            
            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}