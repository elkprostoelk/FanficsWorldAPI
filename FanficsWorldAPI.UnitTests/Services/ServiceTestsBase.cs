using FanficsWorldAPI.Common.Enums;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.UnitTests.Services
{
    public class ServiceTestsBase
    {
        protected readonly FanficsWorldDbContext dbContext;

        protected readonly IRepository<Fanfic> fanficRepository;
        protected readonly IRepository<FanficChapter> fanficChapterRepository;
        protected readonly IRepository<User> userRepository;

        public ServiceTestsBase()
        {
            dbContext = InitializeDbContext();

            fanficRepository = new Repository<Fanfic>(dbContext);
            fanficChapterRepository = new Repository<FanficChapter>(dbContext);
            userRepository = new Repository<User>(dbContext);

            SeedTestData();
        }

        private static FanficsWorldDbContext InitializeDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FanficsWorldDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FanficsWorldDbContext(dbContextOptions);
        }

        private void SeedTestData()
        {
            SeedRoles();
            SeedUsers();
            SeedFanfics();
        }

        private void SeedRoles()
        {
            dbContext.Roles.AddRange([
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Moderator" },
                new Role { Id = 3, Name = "User" }
            ]);

            dbContext.SaveChanges();
        }

        private void SeedUsers()
        {
            dbContext.Users.AddRange([
                new User { Id = "01KANQD02GWS12M5PHBTG5X8DA", Name = "testuser1", Email = "testuser1@mail.com", BirthDate = new DateOnly(1990, 3, 5), IsActive = true, RoleId = 1, PasswordHash = "Lf/lu7+sV6piNonVbBXG2ZJdt5tqXIzp0i2cg2XW6zI=", PasswordSalt = "wlui7OfFDMjaOm9cFXqycg==" },
                new User { Id = "01KANQD038M548FPHN15TXTGN1", Name = "testuser2", Email = "testuser2@mail.com", BirthDate = new DateOnly(1998, 8, 17), IsActive = true, RoleId = 2, PasswordHash = "iZWEdVppUBYwtxgNwpzNIBAOQFUe5TcBrBsxqzGAphI=", PasswordSalt = "LOlLtaBRkUr90GnapbFS6g==" },
                new User { Id = "01KANQD038490G4ZQBP669ABZF", Name = "testuser3", Email = "testuser3@mail.com", BirthDate = new DateOnly(2003, 10, 31), IsActive = true, RoleId = 3, PasswordHash = "bkdQ7mF75nh6qEM1PnS5cry0bHGqEBocH4vK56xAAzM=", PasswordSalt = "mg9OD7bYS7kesyP5UEmwXQ==" },
                new User { Id = "01KANQD038CXC71JMB1WN0SDD7", Name = "testuser4", Email = "testuser4@mail.com", BirthDate = new DateOnly(1980, 1, 2), IsActive = true, RoleId = 3, PasswordHash = "kc37KaXy3KowMZpKkxVWB9DhS7v3K13Cdh8NvFC1moc=", PasswordSalt = "IN+N0SWfGx+jcy36HaryHA==" },
                new User { Id = "01KANQD038TQ4TJFD5E58T220T", Name = "testuser5", Email = "testuser5@mail.com", BirthDate = new DateOnly(2010, 5, 20), IsActive = true, RoleId = 3, PasswordHash = "SYOqUGVLCKz2bRBqht+PvbXREuc7/9TMWUJbEnOoGvM=", PasswordSalt = "2Q9zkNNzMjvGOCC25VSDIQ==" }]);
            dbContext.SaveChanges();
        }

        private void SeedFanfics()
        {
            dbContext.Fanfics.AddRange([
                new Fanfic { Id = "01KANQK7ESG4P91Z6JRQJT4GGM", Title = "Fanfic Title 1", Description = "Description for fanfic 1", Direction = FanficDirection.Gen, Rating = FanficRating.Pg13, Status = FanficStatus.InProgress, AuthorId = "01KANQD038490G4ZQBP669ABZF", CreatedDate = DateTime.UtcNow.AddDays(-10), LastModifiedDate = DateTime.UtcNow.AddDays(-5) },
                new Fanfic { Id = "01KANQK7FA9HPY1ASZ9TY241YN", Title = "Fanfic Title 2", Description = "Description for fanfic 2", Direction = FanficDirection.Slash, Rating = FanficRating.Nc17, Status = FanficStatus.Finished, AuthorId = "01KANQD038CXC71JMB1WN0SDD7", CreatedDate = DateTime.UtcNow.AddDays(-20), LastModifiedDate = DateTime.UtcNow.AddDays(-1) },
                new Fanfic { Id = "01KANQK7FAW9G2WCQ6QNEJGQ4P", Title = "Fanfic Title 3", Description = "Description for fanfic 3", Direction = FanficDirection.Mixed, Rating = FanficRating.R, Status = FanficStatus.Frozen, AuthorId = "01KANQD038TQ4TJFD5E58T220T", CreatedDate = DateTime.UtcNow.AddDays(-15), LastModifiedDate = DateTime.UtcNow.AddDays(-3) }
            ]);
            dbContext.SaveChanges();
        }
    }
}
