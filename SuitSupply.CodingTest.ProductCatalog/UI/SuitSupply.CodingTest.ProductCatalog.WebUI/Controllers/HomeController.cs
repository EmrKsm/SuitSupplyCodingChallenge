using Newtonsoft.Json;
using SuitSupply.CodingTest.ProductCatalog.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers;

namespace SuitSupply.CodingTest.ProductCatalog.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(WebUISettings.Instance.Log4NetName);

        public async Task<ActionResult> Index()
        {
            List<Product> productList = new List<Product>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("BrowseAll");

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;
                        productList = JsonConvert.DeserializeObject<List<Product>>(response);

                    }
                    return View(productList
                        .OrderByDescending(p => p.LastUpdated)
                        .Take(Convert.ToInt32(WebUISettings.Instance.ProductNumberInHome)));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message } );
                }
            }
        }

        public async Task<ViewResult> List(string SearchString, int? page)
        {
            ViewBag.CurrentFilter = SearchString;

            List<Product> productList = new List<Product>();

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("BrowseAll");

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;
                        productList = JsonConvert.DeserializeObject<List<Product>>(response);
                    }
                    if (!String.IsNullOrEmpty(SearchString))
                        productList = productList.Where(p => p.Name.Contains(SearchString)).ToList<Product>();
                    ViewBag.ProductList = productList;
                    TempData["ProductList"] = productList;
                    int pageSize = 6;
                    int pageNumber = (page ?? 1);
                    _logger.Info($"{productList.Count} product listed.");
                    return View(productList.ToPagedList(pageNumber, pageSize));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync($"Browse/{id}");

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;
                        product = JsonConvert.DeserializeObject<Product>(response);
                    }
                    return View(product);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync($"Browse/{id}");

                    if (Res.IsSuccessStatusCode)
                    {
                        var response = Res.Content.ReadAsStringAsync().Result;
                        product = JsonConvert.DeserializeObject<Product>(response);

                    }
                    return View(product);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Product product, List<HttpPostedFileBase> Image)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    byte[] fileData = null;
                    if (Image[0] != null)
                        using (var binaryReader = new BinaryReader(Image[0].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Image[0].ContentLength);
                            product.Photo = fileData;
                        }
                    HttpResponseMessage Res = await client.PostAsJsonAsync($"Edit/{product.Id}", product);

                    if (!Res.IsSuccessStatusCode)
                        return RedirectToAction("Error", new { code = Res.StatusCode, message = Res.ReasonPhrase });
                    _logger.Info($"Product with id {product.Id} edited. New properties; Name:{product.Name} Price:{product.Price} Last Update Time:{product.LastUpdated}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.PostAsync($"Remove/{id}", null);

                    if (!Res.IsSuccessStatusCode)
                        return RedirectToAction("Error", new { code = Res.StatusCode, message = Res.ReasonPhrase });
                    _logger.Info($"Product with id {id} removed.");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(Product product, List<HttpPostedFileBase> Image)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WebUISettings.Instance.ProductWebApiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    byte[] fileData = null;
                    if (Image[0] != null)
                        using (var binaryReader = new BinaryReader(Image[0].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Image[0].ContentLength);
                            product.Photo = fileData;
                        }
                    HttpResponseMessage Res = await client.PostAsJsonAsync("Add", product);

                    if (!Res.IsSuccessStatusCode)
                        return RedirectToAction("Error", new { code = Res.StatusCode, message = Res.ReasonPhrase });
                    _logger.Info($"Product {product.Name} added.");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return View("Error", new { code = -1, message = ex.Message });
                }
            }
        }

        public ActionResult ExportDataToExcel()
        {
            try
            {
                List<Product> products = (List<Product>)TempData["ProductList"];
                var grid = new GridView();
                grid.DataSource = products;
                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachement; filename=ProductList.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                _logger.Info($"Product list exported as excel file including {products.Count} products.");
                return View("List", products);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error", new { code = -1, message = ex.Message });
            }
        }

        public ActionResult Error(string code, string message)
        {
            ViewBag.ErrorCode = code;
            ViewBag.ErrorMessage = message;

            _logger.Error($"Error code: {code} | Error message: {message}");

            return View();
        }
    }
}