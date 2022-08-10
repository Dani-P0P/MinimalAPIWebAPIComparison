namespace Minimal_WebApiComparison.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {

        }

        public DbSet<Person> Persons => Set<Person>();
    }
}
