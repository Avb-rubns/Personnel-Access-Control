namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class UserRepository : IUserRepositoryEFC
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
                user.Status = userFinded.Status;
                user.LastName = userFinded.LastName;
            }

            return user;
        }

        public async Task<List<UserRegistedDTO>> GetAllUsersforPageAsync(int? page, int? pagesize)
        {
            List<UserRegistedDTO> users = new();
            int pageSize = (pagesize.Value > 0 ? pagesize.Value : 25);
            int pageNumber = (page.Value > 0 ? page.Value : 1);

            var data = await Context.Users.OrderBy(s => s.UserID)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();

            if (data.Count() > 0)
            {
                users = data.Select(s =>
                    new UserRegistedDTO
                    {
                        UserID = s.UserID,
                        UserName = s.Name,
                        LastName = s.LastName,
                        Phone = s.Phone,
                        Email = s.Email,
                        Status = s.Status,
                        RolID = s.RolID

                    })
                    .ToList();

            }

            return users;
        }
    }
}
