using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Controllers
{
    public class AddController : ApiController
    {
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(WebApiSettings.Instance.Log4NetName);

        [Route("api/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody()]Product product)
        {
            try
            {
                using (var p = new Product())
                {
                    var result = await Task.FromResult(p.AddProduct(product));
                    if (result)
                        return Ok();
                    else
                        return Content(HttpStatusCode.BadRequest, $"Cannot add product with name {product.Name}");
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
