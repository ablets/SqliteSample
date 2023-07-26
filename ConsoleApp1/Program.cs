using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SqliteSample
{
    class Program
    {
        static void Main()
        {
            var databasePath = "myDb.db";

            using var connection = new SqliteConnection($"Data Source={databasePath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Age INTEGER NOT NULL)";
            command.ExecuteNonQuery();

            var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite($"Data Source={databasePath}").Options;
            using var db = new MyDbContext(options);

            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter your age:");
            int age = int.Parse(Console.ReadLine()!);
            var user = new User { Name = name, Age = age };
            db.Users.Add(user);
            db.SaveChanges();

            Console.WriteLine("All users:");
            foreach (var u in db.Users)
                Console.WriteLine($"{u.Id}: {u.Name} ({u.Age})");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
    }

    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

