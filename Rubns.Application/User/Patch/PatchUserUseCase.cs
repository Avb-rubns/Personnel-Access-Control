namespace Rubns.Application.User.Patch
{
    internal sealed class PatchUserUseCase(IUserRepositoryEFC repositoryEFC, ILogger logger) : IPatchUserPort
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task<int> PatchUserAsync(int userID, JsonPatchDocument<UserRegistedDTO> patchDocument)
        {
            try
            {
                int resulrUpdate = 0;
                var userFind = await _repositoryEFC.FindUserforIDAsync(userID);

                if (userFind is { UserID: <= 0 })
                {
                    return -1;

                }

                var user = new UserRegistedDTO();

                patchDocument.ApplyTo(user);

                userFind.UserName = userFind.UserName != user.UserName ? user.UserName : null;
                userFind.LastName = userFind.LastName != user.LastName ? user.LastName : null;
                userFind.Phone = userFind.Phone != user.Phone ? user.Phone : null;
                userFind.Email = userFind.Email != user.Email ? user.Email : null;
                userFind.RolID = userFind.RolID == userFind.RolID ? user.RolID : userFind.RolID;
                userFind.Status = userFind.Status == userFind.Status ? user.Status : userFind.Status;

                resulrUpdate = await _repositoryEFC.UpdateUserforIDAsync(userFind);
                return resulrUpdate;


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error Update User: {error}", ex.Message);
            }

            return 0;

        }
    }
}
