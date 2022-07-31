//using QueryDataApi.Model;
//using QueryDataApi.Respository;
//using QueryTestQuality;

//namespace QueryQuality
//{
//    public class QuertServiceFake : IRepository
//    {
//        private readonly List<QueryModel> _queries;
//        public QuertServiceFake(QueryDataHelper queryDataHelper)
//        {
//            _queries = queryDataHelper.LoadQueryFakeData(_queries);
//        }

//        public Task<List<QueryModel>> GetQueries()
//        {
//            return Task.FromResult(_queries);
//        }

//        public Task<QueryModel> GetQueryById(long queryId) => Task.FromResult(_queries.FirstOrDefault(a => a.Id == queryId));

//        public Task<int> AddQuery(QueryModel post)
//        {
//            _queries.Add(post);
//            return Task.FromResult(int.Parse(post.QueryRef));
//        }

//        public Task<int> DeleteQuery(long queryId)
//        {
//            var existing = _queries.FirstOrDefault(a => a.Id == queryId);
//            if (existing == null) return Task.FromResult(0);

//            _queries.Remove(existing);
//            return Task.FromResult(int.Parse(existing.QueryRef));
//        }

//        public Task<int> UpdateQuery(long queryId, QueryModel post)
//        {
//            var existing = _queries.FirstOrDefault(a => a.Id == queryId);
//            if (existing == null) return Task.FromResult(0);
//            existing.QueryRef = post.QueryRef;
//            existing.QueryType = post.QueryType;
//            existing.Status = post.Status;
//            existing.RaisedBy = post.RaisedBy;
//            existing.RaisedOn = post.RaisedOn;
//            return Task.FromResult(existing.Id);
//        }       
//    }
//}
