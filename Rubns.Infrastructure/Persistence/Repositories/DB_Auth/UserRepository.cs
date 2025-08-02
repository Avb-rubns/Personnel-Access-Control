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
            var userFinded = await Context.Users.Where(s =>
                                    s.Email == registerUser.Email
                                    || s.Phone == registerUser.Phone)
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
            int pageSize = ((pagesize.HasValue && pagesize.Value > 0) ? pagesize.Value : 25);
            int pageNumber = ((page.HasValue && page.Value > 0) ? page.Value : 1);

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

        public async Task<UserDTO> FindUserforIDAsync(int ID)
        {
            UserDTO user = new();
            var userFinded = await Context.Users.Where(s => s.UserID == ID)
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
                user.RolID = userFinded.RolID;
            }

            return user;
        }

        public async Task<int> UpdateUserforIDAsync(UserDTO user)
        {
            User updateUser = new()
            {
                UserID = user.UserID,
                Name = user.UserName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                RolID = user.RolID
            };

            Context.Entry(updateUser).Property(u => u.Name).IsModified = !string.IsNullOrEmpty(user.UserName);
            Context.Entry(updateUser).Property(u => u.LastName).IsModified = !string.IsNullOrEmpty(user.LastName);
            Context.Entry(updateUser).Property(u => u.Email).IsModified = !string.IsNullOrEmpty(user.Email);
            Context.Entry(updateUser).Property(u => u.Phone).IsModified = !string.IsNullOrEmpty(user.Phone);
            Context.Entry(updateUser).Property(u => u.Status).IsModified = true;
            Context.Entry(updateUser).Property(u => u.RolID).IsModified = true;

            return await Context.SaveChangesAsync();


        }

        public async Task<int> TotalUsersAsync()
        {
            int total = 0;

            total = await Context.Users.CountAsync();

            return total;
        }

        public async Task<TableUserDTO> FindUserAsync(string search, int? page, int? pagesize)
        {
            TableUserDTO tableUsers = new();
            var query = Context.Users.AsQueryable();
            int pageSize = ((pagesize.HasValue && pagesize.Value > 0) ? pagesize.Value : 25);
            int pageNumber = ((page.HasValue && page.Value > 0) ? page.Value : 1);

            var user = await query.Where(s => s.Name.Contains(search)
                            || s.LastName.Contains(search)
                            || s.Phone.Contains(search))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .OrderBy(s => s.UserID)
                .ToListAsync();

            if (user.Count() > 0)
            {
                tableUsers.RegisteredUsers = user.Select(s => new UserRegistedDTO()
                {
                    UserID = s.UserID,
                    UserName = s.Name,
                    LastName = s.LastName,
                    Phone = s.Phone,
                    Email = s.Email,
                    RolID = s.RolID,
                    Status = s.Status,
                }).ToList();

                tableUsers.Total = user.Count();
            }

            return tableUsers;


        }

        public async Task<UserDTO> FindUserforEmailAsync(string email)
        {
            UserDTO user = new();
            var userFinded = await Context.Users.Where(s =>
                                    s.Email == email)
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
    }
}
