using QueryDataApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueryDataApi.Respository
{
    public interface IRepository<T>
    {
        Task<List<T>> GetQueries();

        Task<T> GetQueryById(long queryId);

        Task<int> AddQuery(T post);

        Task<int> DeleteQuery(long postId);

        Task<int> UpdateQuery(long id, T post);
    }
}
