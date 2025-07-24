namespace Rubns.Core.Ports.Template
{
    public interface ITemplateRepositoryDapper
    {
        Task<string> GetMailRegistedAsync(RegisterUserDTO registerUser);
    }
}
