using MongoDB.Driver;
using UserAuthBackend.Data;
using UserAuthBackend.Models;

namespace UserAuthBackend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<User> RegisterAsync(string username, string password)
        {
            var existingUser = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (existingUser != null) return null;

            Console.WriteLine($"Username: {username}");
            Console.WriteLine($"Password: {password}");

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "Username não pode ser nulo ou vazio.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password não pode ser nulo ou vazio.");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }
    }
}
