using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Controllers
{
    public class RemoveController : ApiController
    {
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(WebApiSettings.Instance.Log4NetName);

        [Route("api/Remove/{id:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromUri()]int id)
        {
            try
            {
                using (var p = new Product())
                {
                    var result = await Task.FromResult(p.RemoveProduct(id));
                    if (result)
                        return Ok();
                    else
                        return Content(HttpStatusCode.BadRequest, $"Cannot remove product with id {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }
    }
}
