using System.Data.Entity;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Models.Database
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(string connectionString) : base(connectionString){}
        public DbSet<Product> Product { get; set; }
    }
}