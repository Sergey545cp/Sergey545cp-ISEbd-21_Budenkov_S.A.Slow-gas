using Microsoft.EntityFrameworkCore;
using SlowGas.Database;

namespace SlowGas.Tests.StoragesContractsTests
{
    public class BaseStorageTest : IDisposable
    {
        protected readonly SlowGasDbContext _dbContext;
        protected readonly string _connectionString = "Host=localhost;Database=SlowGasTest;Username=postgres;Password=postgres";

        public BaseStorageTest()
        {
            var options = new DbContextOptionsBuilder<SlowGasDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            _dbContext = new SlowGasDbContext(options);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}