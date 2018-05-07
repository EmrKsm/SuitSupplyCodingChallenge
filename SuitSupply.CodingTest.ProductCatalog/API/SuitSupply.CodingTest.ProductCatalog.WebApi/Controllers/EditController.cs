using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Controllers
{
    public class EditController : ApiController
    {
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(WebApiSettings.Instance.Log4NetName);

        [Route("api/Edit/{id:int}")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromUri()]int id, [FromBody()]Product product)
        {
            try
            {
                using (var p = new Product())
                {
                    var result = await Task.FromResult(p.EditProduct(id, product));
                    if (result)
                        return Ok();
                    else
                        return Content(HttpStatusCode.BadRequest, $"Cannot edit product with id {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        #region Helpers
        private  bool CheckIfTwoProductsEqual(IProduct source, Product incoming)
        {
            return source.Id == incoming.Id && source.Name == incoming.Name && source.Photo == incoming.Photo && source.Price == incoming.Price && source.LastUpdated == incoming.LastUpdated;
        }
        #endregion
    }
}
