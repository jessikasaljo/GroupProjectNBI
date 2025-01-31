namespace Application.Interfaces
{
    public interface ITokenHelper
    {
        string GenerateToken(Domain.Models.User user);
    }
}