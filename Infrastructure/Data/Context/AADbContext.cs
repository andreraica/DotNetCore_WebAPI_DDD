//using System.IO;
using Domain.Entities;
using Infrastructure.Data.Extensions;
using Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class AADbContext : DbContext
    {
        public AADbContext(DbContextOptions<AADbContext> options)
            : base(options)
        {
        }

        public DbSet<Participant> Participant {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ForSqlServerUseIdentityColumns();
            builder.AddConfiguration(new ParticipantMap());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = CoreApiDDD; Integrated Security = True; MultipleActiveResultSets = True");
            //optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }
    }
}
