using Microsoft.EntityFrameworkCore;
using ModelAPI.Models;

namespace ModelAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Expense> Expenses { get; set; } = default!;

        public DbSet<Job> Jobs { get; set; } = default!;

        public DbSet<Model> Models { get; set; } = default!;
    }
}
