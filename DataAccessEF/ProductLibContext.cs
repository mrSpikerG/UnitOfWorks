using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessEF {
    public class ProductLibContext : DbContext {
        public DbSet<Product> Products { get; set; }

        public ProductLibContext(DbContextOptions options) : base(options) {
            
        }

        
    }
}