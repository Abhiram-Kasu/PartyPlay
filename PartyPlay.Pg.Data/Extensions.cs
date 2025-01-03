using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PartyPlay.Pg.Data;

public static class Extensions
{
    public static void AddPgFeatures(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("partyPlay");
        builder.Services.AddScoped<IPartyRepository, PgPartyRepository>();
        builder.Services.AddScoped<IPartyUserRepository, PgPartyUserRepository>();

    }
    
}