namespace Rubns.Application.UseCases.Users.Commands
{
    internal sealed class UpdateUserUseCase(IUserRepositoryEFC repositoryEFC,
        ILogger logger)
        : IUpdateUserUseCase
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task Executeasync(int userID, JsonPatchDocument<UserRegistedDTO> patchDocument)
        {
            try
            {
                int resulrUpdate = 0;
                var userFind = await _repositoryEFC.FindUserforIDAsync(userID);

                if (userFind is { UserID: <= 0 })
                {
                    throw new NotFoundException($"El usuario no existe{userID}", "Sin informacion");

                }

                var user = new UserRegistedDTO();

                patchDocument.ApplyTo(user);

                userFind.Name = userFind.Name != user.UserName ? user.UserName : null;
                userFind.LastName = userFind.LastName != user.LastName ? user.LastName : null;
                userFind.Phone = userFind.Phone != user.Phone ? user.Phone : null;
                userFind.Email = userFind.Email != user.Email ? user.Email : null;
                userFind.RolID = userFind.RolID == userFind.RolID ? user.RolID : userFind.RolID;
                userFind.Status = userFind.Status == userFind.Status ? user.Status : userFind.Status;

                resulrUpdate = await _repositoryEFC.UpdateUserforIDAsync(userFind);
                if (resulrUpdate <= 0)
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error Update User: {error}", ex.Message);
                throw;
            }

        }
    }
}
