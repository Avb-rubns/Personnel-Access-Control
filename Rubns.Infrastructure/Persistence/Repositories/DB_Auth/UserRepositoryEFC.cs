namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class UserRepositoryEFC : IUserRepositoryEFC
    {
        AuthDbContextEFC Context { get; set; }

        public UserRepositoryEFC(AuthDbContextEFC context)
        {
            Context = context;
        }

        public async Task<int> RegisterAsync(User registerUser
            , string password)
        {
            UserDb user = new()
            {
                Name = registerUser.Name,
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

        public async Task<User> FindUserByPhoneOrEmailAsync(string email, string phone)
        {
            User user = new();
            var userFinded = await Context.Users.Where(s =>
                                    s.Email == email
                                    || s.Phone == phone)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync();

            if (userFinded is { UserID: > 0 })
            {
                user.UserID = userFinded.UserID;
                user.Name = userFinded.Name;
                user.Phone = userFinded.Phone;
                user.Email = userFinded.Email;
                user.Status = userFinded.Status;
                user.LastName = userFinded.LastName;
            }

            return user;
        }

        public async Task<List<User>> GetAllUsersforPageAsync(int? page, int? pagesize)
        {
            List<User> users = new();
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
                    new User
                    {
                        UserID = s.UserID,
                        Name = s.Name,
                        LastName = s.LastName,
                        Phone = s.Phone,
                        Email = s.Email,
                        Status = s.Status,
                        RolID = s.RolID.Value,

                    })
                    .ToList();

            }

            return users;
        }

        public async Task<User> FindUserforIDAsync(int ID)
        {
            User user = new();
            var userFinded = await Context.Users.Where(s => s.UserID == ID)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync();

            if (userFinded is { UserID: > 0 })
            {
                user.UserID = userFinded.UserID;
                user.Name = userFinded.Name;
                user.Phone = userFinded.Phone;
                user.Email = userFinded.Email;
                user.Status = userFinded.Status;
                user.LastName = userFinded.LastName;
                user.RolID = userFinded.RolID.Value;
            }

            return user;
        }

        public async Task<int> UpdateUserforIDAsync(User user)
        {
            User updateUser = new()
            {
                UserID = user.UserID,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                RolID = user.RolID
            };

            Context.Entry(updateUser).Property(u => u.Name).IsModified = !string.IsNullOrEmpty(user.Name);
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

        public async Task<List<User>> FindUsersAsync(string search, int? page, int? pagesize)
        {
            List<User> tableUsers = new();
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
                tableUsers = user.Select(s => new User()
                {
                    UserID = s.UserID,
                    Name = s.Name,
                    LastName = s.LastName,
                    Phone = s.Phone,
                    Email = s.Email,
                    RolID = s.RolID.Value,
                    Status = s.Status,
                }).ToList();

            }

            return tableUsers;


        }

        public async Task<User> FindUserforEmailAsync(string email)
        {
            User user = new();
            var userFinded = await Context.Users.Where(s =>
                                    s.Email == email)
                                    .AsNoTracking()
                                    .SingleOrDefaultAsync();

            if (userFinded is { UserID: > 0 })
            {
                user.UserID = userFinded.UserID;
                user.Name = userFinded.Name;
                user.Phone = userFinded.Phone;
                user.Email = userFinded.Email;
                user.Status = userFinded.Status;
                user.LastName = userFinded.LastName;
            }

            return user;
        }
    }
}
