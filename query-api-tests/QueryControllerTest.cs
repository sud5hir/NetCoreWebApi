//using QueryDataApi.Controllers;
//using QueryDataApi.Model;
//using QueryDataApi.Respository;
//using Microsoft.AspNetCore.Mvc;
//using QueryQuality;
//using System.Collections.Generic;
//using Xunit;
//using QueryTestQuality;

//namespace query_api_tests
//{
//    public class QueryControllerTest
//    {
//        private readonly QueryController _controller;
//        private readonly IRepository _service;
//        private readonly QueryDataHelper _queryDataHelper;

//        public QueryControllerTest()
//        {
//            _queryDataHelper = new QueryDataHelper();
//            _service = new QuertServiceFake(_queryDataHelper);
//            _controller = new QueryController(_service);
//        }

//        [Fact]
//        public void When_GetQueryData_ReturnsOkResult()
//        {

//            var queryResult = _controller.Get();

//            Assert.IsType<OkObjectResult>(queryResult.Result);

//            var querylist = queryResult.Result as OkObjectResult;

//            Assert.IsType<List<QueryModel>>(querylist.Value);

//            var queries = querylist.Value as List<QueryModel>;

//            Assert.Equal(4, queries.Count);
//        }


//        [Theory]
//        [InlineData(1)]
//        [InlineData(2)]
//        public void When_GetQueryData_WithId_ReturnsOkResult(long queryId)
//        {

//            var queryResult = _controller.GetQueryById(queryId);

//            Assert.IsType<OkObjectResult>(queryResult.Result);

//            var querylist = queryResult.Result as OkObjectResult;

//            Assert.IsType<QueryModel>(querylist.Value);

//            var query = querylist.Value as QueryModel;

//            Assert.Equal(queryId, query.Id);
//        }

//        [Theory]
//        [InlineData(5)]
//        [InlineData(12)]
//        public void When_GetQueryData_WithId_ReturnsNotFoundResult(long queryId)
//        {
//            var queryResult = _controller.GetQueryById(queryId);

//            Assert.IsType<NotFoundResult>(queryResult.Result);
//        }

//        [Fact]
//        public void RemoveQueryData_NotExistingQueryRef_ReturnsNotFoundResponse()
//        {

//            var queryId = 5;

//            var badResponse = _controller.DeleteQuery(queryId);

//            Assert.IsType<NotFoundResult>(badResponse.Result);
//        }

//        [Fact]
//        public void RemoveQueryData_ExistingQueryRef_ReturnsNotFoundResponse()
//        {

//            var queryId = 3;

//            var noContentResponse = _controller.DeleteQuery(queryId);

//            Assert.IsType<NoContentResult>(noContentResponse.Result);

//            Assert.Equal(3, _service.GetQueries().Result.Count);
//        }


//        [Fact]
//        public void UpdateQueryData_NotExistingQueryRef_ReturnsNotFoundResponse()
//        {

//            var queryId = 5;

//            var badResponse = _controller.Put(queryId, new QueryModel());

//            Assert.IsType<NotFoundResult>(badResponse.Result);
//        }

//        [Fact]
//        public void UpdateQueryData_ExistingQueryRef_ReturnsNotFoundResponse()
//        {

//            var queryId = 3;
//            var queryModel = new QueryModel
//            {
//                QueryType = "General Query"
//            };

//            var okResponse = _controller.Put(queryId, queryModel);

//            Assert.IsType<OkResult>(okResponse.Result);

//            Assert.Equal("General Query", _service.GetQueryById(3).Result.QueryType);
//        }

//        [Fact]
//        public void AddQueryData_ReturnsOkResponse()
//        {

//            var queryModel = new QueryModel
//            {
//                QueryRef = "4",
//                QueryType = "General Query",
//                RaisedBy = "Monu",
//                RaisedOn = "14 Aug 2021",
//                Status = "Open"
//            };

//            var okResponse = _controller.Post(queryModel);

//            Assert.IsType<OkResult>(okResponse.Result);

//            Assert.Equal(5, _service.GetQueries().Result.Count);
//        }
//    }
//}