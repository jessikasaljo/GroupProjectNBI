using Domain.Models;

namespace Infrastructure.Database
{
    public interface IDatabase
    {
        Task<Dictionary<int, T>> GetAll<T>(string table);
        User? LoginUser(string username, string password);
        Task<T> GetById<T>(string table, int id);
        Task<bool> Add<T>(string table, T itemToAdd);
        Task<bool> UpdateById<T>(string table, int id, T itemToUpdate);
        Task<bool> DeleteById(string table, int id);
    }
}
