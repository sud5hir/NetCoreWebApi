using QueryDataApi.Model;
using Microsoft.EntityFrameworkCore;

namespace QueryDataApi.Dal
{
    public class QueryContextDal : DbContext
    {
        public QueryContextDal()
        { }              

        public DbSet<QueryModel> Quries { get; set; }

        public QueryContextDal(DbContextOptions options) : base(options)
        {

        }
    }
}
