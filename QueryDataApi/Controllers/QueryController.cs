using QueryDataApi.Model;
using QueryDataApi.Respository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace QueryDataApi.Controllers
{
    [Route("[controller]")]
    public class QueryController : Controller
    {
        IRepository<QueryModel> _queryRepository;

        public object Configuration { get; private set; }

        public QueryController(IRepository<QueryModel> queryRepository)
        {
            _queryRepository = queryRepository;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {
                var queries = await _queryRepository.GetQueries();

                if (queries == null)
                {
                    return NotFound();
                }
                return Ok(queries);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{queryId}")]
        public async Task<IActionResult> GetQueryById(long queryId)
        {
            try
            {
                var query = await _queryRepository.GetQueryById(queryId);
                if (query == null)
                {
                  
                    return NotFound();
                }
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QueryModel queryModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            try
            {
                int result = await _queryRepository.AddQuery(queryModel);
                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{queryId}")]
        public async Task<IActionResult> Put(long queryId, [FromBody] QueryModel queryModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            try
            {
                var res = await _queryRepository.UpdateQuery(queryId, queryModel);

                if (res == 0)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }
        }

        [HttpDelete("{queryId}")]
        public async Task<IActionResult> DeleteQuery(long queryId)
        {
            try
            {

                var result = await _queryRepository.DeleteQuery(queryId);
                if (result == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
