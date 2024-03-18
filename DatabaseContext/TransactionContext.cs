using Microsoft.EntityFrameworkCore;
using TransactionApi.Entities;

namespace TransactionApi.DatabaseContext
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
