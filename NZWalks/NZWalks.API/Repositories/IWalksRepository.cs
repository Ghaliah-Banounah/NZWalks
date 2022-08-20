using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walks>> GetAllAsync();

        Task<Walks> GetAsync(Guid id);

        Task<Walks> AddAsync(Walks walk);

        Task<Walks> DeleteAsync(Guid id);

        Task<Walks> UpdateAsync(Guid id, Walks walk);
    }
}
