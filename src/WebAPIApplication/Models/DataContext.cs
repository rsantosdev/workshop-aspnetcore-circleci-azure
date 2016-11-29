using Microsoft.EntityFrameworkCore;

namespace WebAPIApplication {
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Pessoa> Pessoas { get; set; }
  }
}