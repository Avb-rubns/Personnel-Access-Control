namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class UserRepository : IUserRepository
    {
        AuthDbContextEFC Context { get; set; }

        public UserRepository(AuthDbContextEFC context)
        {
            Context = context;
        }

        public async Task<int> RegisterAsync(RegisterUserDTO registerUser
            ,string password)
        {
            User user = new() 
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                Password = password

            
            };

            await Context.Users.AddAsync(user);
            return await Context.SaveChangesAsync();
        }
    }
}
