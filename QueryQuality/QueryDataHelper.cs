using QueryDataApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryTestQuality
{
    public  class QueryDataHelper
    {
        public List<QueryModel>  LoadQueryFakeData(List<QueryModel> _queries)
        {
            var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"TestData\\{"Query.json"}");

            var queryFile = System.IO.File.ReadAllText(folderDetails);

            var quries = Newtonsoft.Json.JsonConvert.DeserializeObject<Queries>(queryFile);
            _queries = new List<QueryModel>();
            _queries.AddRange(quries.QueryList);

            return _queries;
        }
    }
}
