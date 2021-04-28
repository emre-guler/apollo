using Microsoft.EntityFrameworkCore;

namespace Apollo.Data 
{
    public class ApolloContext : DbContext 
    {
        public ApolloContext (DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Özel veri girişi
        }

    }
}