using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //build ti fa aggiungere i servizi di ef o creati da te

        // Add services to the container.
        builder.Services.AddControllers();
        //lo copio nella classe dedicata ai servizi dell'applicazione quindi
        //dentro extensions --> ApplicationServiceExtensions
        /* builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        }); 
        builder.Services.AddCors();
        builder.Services.AddScoped<ITokenService, TokenServices>(); */
        builder.Services.AddApplicationServices(builder.Configuration);

        //lo copio nei servizi per entiti quindi
        //dento extensions --> IdentityServiceExtensions
        /* builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,  
                };
            }); */
        //dopo JwtBearerDefaults devo dichiarare dove lo voglio
        //concatenare nel nostro caso dopo AuthenticationScheme
        //quindi poi vado a specificare a bearer alcune opzioni
        builder.Services.AddIdentityServices(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        //middleweare che gestisce gli errori
        //<ExceptionMiddleware> è il nome del file del middleware che ho appe creato
        app.UseMiddleware<ExceptionMiddleware>();
        //middleweare ed è molto importante dove si posiziona
        //solitamente prima dei controller
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

        app.UseAuthentication(); //chiede se l'autenticazione è valida 
        app.UseAuthorization(); //chiede se è autorizzato ad avere quell'autorizzazione esempio a 16 anni hai un documento valido ma non sei autorizzato a comprare sigarette

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(context);
        }
        catch (Exception ex)
        {
             var logger = services.GetRequiredService<ILogger<Program>>();
             logger.LogError(ex, "errore durante la migrazione");
        }


        app.Run();
    }
}