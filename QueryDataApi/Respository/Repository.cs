using Microsoft.EntityFrameworkCore;
using QueryDataApi.Dal;
using QueryDataApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryDataApi.Respository
{
    public class Repository<T> : DbContext, IRepository<T>
          where T : QueryModel
    {
        QueryContextDal _queryContextDal;
        private DbSet<T> entities;

        public Repository(QueryContextDal queryContextDal)
        {
            _queryContextDal = queryContextDal;
            entities = _queryContextDal.Set<T>();
        }

        public async Task<int> AddQuery(QueryModel query)
        {
            var lastQueryId = _queryContextDal.Quries.LastOrDefault().Id;
            query.Id = lastQueryId + 1;

            try
            {
                await _queryContextDal.Quries.AddAsync(query);
                await _queryContextDal.SaveChangesAsync();
                return query.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> AddQuery(T post)
        {
            QueryModel query = (QueryModel)post;
            var lastQueryId = _queryContextDal.Quries.LastOrDefault().Id;
            query.Id = lastQueryId + 1;

            try
            {
                await _queryContextDal.Quries.AddAsync(query);
                await _queryContextDal.SaveChangesAsync();
                return query.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteQuery(long queryId)
        {
            var existingStudent = _queryContextDal.Quries.FirstOrDefault(s => s.Id == queryId);
            if (existingStudent == null)
            {
                return 0;
            }

            _queryContextDal.Quries.Remove(existingStudent);
            var result = await _queryContextDal.SaveChangesAsync();
            return result;
        }

        public Task<List<QueryModel>> GetQueries()
        {
            var queries = _queryContextDal.Quries;
            if (queries == null)
            {
                return null;
            }
            return Task.FromResult(queries.ToList());
        }

        public Task<QueryModel> GetQueryById(long queryId)
        {
            var query = _queryContextDal.Quries.FirstOrDefault(x => x.Id == queryId);
            if (query == null)
            {
                return null;
            }
            return Task.FromResult(query);
        }

        public async Task<int> UpdateQuery(long queryId, QueryModel query)
        {
            var existingStudent = _queryContextDal.Quries.FirstOrDefault(s => s.Id == queryId);
            if (existingStudent == null)
            {
                return 0;
            }

            existingStudent.Status = query.Status;
            try
            {
                
                _queryContextDal.Quries.Update(existingStudent);
                var res = await _queryContextDal.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateQuery(long id, T post)
        {
            QueryModel query = (QueryModel)post;
            var existingStudent = _queryContextDal.Quries.FirstOrDefault(s => s.Id == query.Id);
            if (existingStudent == null)
            {
                return 0;
            }

            existingStudent.Status = query.Status;
            try
            {

                _queryContextDal.Quries.Update(existingStudent);
                var res = await _queryContextDal.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Task<List<T>>  IRepository<T>.GetQueries()
        {
            return Task.FromResult(entities.ToList());
            // return Task.FromResult(Set<T>().ToList());

        }

        Task<T> IRepository<T>.GetQueryById(long queryId)
        {
            return null;
        }
    }
}
