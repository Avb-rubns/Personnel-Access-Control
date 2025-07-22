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
            , string password)
        {
            User user = new()
            {
                Name = registerUser.UserName,
                Email = registerUser.Email,
                Password = password,
                Phone = registerUser.Phone,
                LastName = registerUser.LastName,
                RolID = registerUser.RolID,
                Status = registerUser.Status,

            };

            await Context.Users.AddAsync(user);
            return await Context.SaveChangesAsync();
        }

        public async Task<UserDTO> FindUserAsync(RegisterUserDTO registerUser)
        {
            UserDTO user = new();
            var userFinded = await Context.Users.Where(s => s.Email == registerUser.Email || s.Phone == registerUser.Phone)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync();

            if (userFinded is { UserID: > 0 })
            {
                user.UserID = userFinded.UserID;
                user.UserName = userFinded.Name;
                user.Phone = userFinded.Phone;
                user.Email = userFinded.Email;
                user.Status = (short)(userFinded.Status ? 1 : 0);
                user.LastName = userFinded.LastName;
            }

            return user;
        }
    }
}
