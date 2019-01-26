using System.Collections.Generic;
using System.Threading.Tasks;
using TRP.Models;

namespace TRP.Services
{
    //public interface IDataStore<T> where T : class
    //{
    //    Task<bool> AddAsync(T item);
    //    Task<bool> UpdateAsync(T item);
    //    Task<bool> DeleteAsync(T item);
    //    Task<T> GetAsync(string id);
    //    Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);

    //}

    public interface IDataStore
    {
        Task<bool> InsertUpdateAsync_Item(Item data);
        Task<bool> AddAsync_Item(Item data);
        Task<bool> UpdateAsync_Item(Item data);
        Task<bool> DeleteAsync_Item(Item data);
        Task<Item> GetAsync_Item(string id);
        Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false);

        // Implement Monster
        // Implement Character
        Task<bool> InsertUpdateAsync_Character(Character data);
        Task<bool> AddAsync_Character(Character data);
        Task<bool> UpdateAsync_Character(Character data);
        Task<bool> DeleteAsync_Character(Character data);
        Task<Character> GetAsync_Character(string id);
        Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false);
        // Implement Score

    }
}
