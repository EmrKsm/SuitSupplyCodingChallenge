using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Controllers
{
    public class BrowseController : ApiController
    {
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(Environment.MachineName);

        [Route("api/BrowseAll")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                using (var product = new Product())
                {
                    return Ok(await Task.FromResult(product.GetAllProducts()));
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("api/Browse/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri()]int id)
        {
            try
            {
                using (var product = new Product())
                {
                    return Ok(await Task.FromResult(product.GetProductById(id)));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
