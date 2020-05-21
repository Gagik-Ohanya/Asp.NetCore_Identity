using Enyim.Caching.Memcached.Results;
using System.Threading.Tasks;

namespace BLL.Caching
{
    public interface ICacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);

        Task<IGetOperationResult<T>> GetCache<T>(string key);
        Task SetCache<T>(string key, T value);
    }
}