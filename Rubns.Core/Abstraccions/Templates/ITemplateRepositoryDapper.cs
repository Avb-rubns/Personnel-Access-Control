namespace Rubns.Core.Abstraccions.Templates
{
    public interface ITemplateRepositoryDapper
    {
        Task<string> GetMailRegistedAsync(RegisterUserDTO registerUser);
        Task<string> GetMailForgotPasswordAsync(string nameUser, string token);
    }
}
