using Microsoft.EntityFrameworkCore;

namespace TransactionApi
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
