using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Controllers;
using System.Web.Http.Results;
using System.Net;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Test.Add
{
    public class ControllerTest
    {
        [TestFixture]
        public class Controller_AddNewProduct
        {
            List<Image> photos = null;
            AddController controller = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                controller = new AddController();
                foreach (var file in Directory.GetFiles(@"Z:\productcatalog"))
                {
                    photos.Add(Image.FromFile(file));
                }
            }

            [Test]
            public void Controller_AddFromController_ShouldAddProductAndReturnTrue()
            {
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    Random rnd = new Random();
                    photos[rnd.Next(0, photos.Count - 1)].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }
                Product product = new Product { Name = "Yellow suit2112121212", Photo = arr, Price = 11221.0, LastUpdated = DateTime.Now };

                var result = controller.Post(product).Result;

                if (result.GetType() == typeof(OkResult))
                    Assert.Pass();
                else
                {
                    var contentResult = (NegotiatedContentResult<string>)result;
                    Assert.Fail(contentResult.Content);
                }
            }
        }

        [TestFixture]
        public class Controller_GetProduct
        {
            List<Image> photos = null;
            BrowseController controller = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                controller = new BrowseController();
                foreach (var file in Directory.GetFiles(@"Z:\productcatalog"))
                {
                    photos.Add(Image.FromFile(file));
                }
            }

            [Test]
            public void Controller_GetAllProductsFromController_ShouldReturnAllProducts()
            {
                var productListContent = ((OkNegotiatedContentResult<List<IProduct>>)controller.Get().Result).Content ;
                Assert.NotNull(productListContent);
            }

            [TestCase(1)]
            [TestCase(3)]
            [TestCase(4)]
            [TestCase(8)]
            [TestCase(10)]
            public void GetProductByIdFromController_ShouldReturnProduct(int id)
            {
                var product = ((OkNegotiatedContentResult<IProduct>)controller.GetById(id).Result).Content;
                Assert.NotNull(product);
            }
        }

        [TestFixture]
        public class Controller_RemoveProduct
        {
            List<Image> photos = null;
            RemoveController controller = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                controller = new RemoveController();
            }

            [TestCase(12)]
            [TestCase(11)]
            public void Controller_RemoveFromController_ShouldRemoveProductAndReturnTrue(int id)
            {
                var result = controller.Post(id).Result;

                if (result.GetType() == typeof(OkResult))
                    Assert.Pass();
                else
                {
                    var contentResult = (NegotiatedContentResult<string>)result;
                    Assert.Fail(contentResult.Content);
                }
            }
        }

        [TestFixture]
        public class Controller_Edit
        {
            List<Image> photos = null;
            EditController controller = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                controller = new EditController();
                foreach (var file in Directory.GetFiles(@"Z:\productcatalog"))
                {
                    photos.Add(Image.FromFile(file));
                }
            }

            [Test]
            public void Controller_EditFromController_ShouldEditProductsPropertiesAndReturnTrue()
            {
                int id = 2;
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    Random rnd = new Random();
                    photos[rnd.Next(0, photos.Count - 1)].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }

                Product product = new Product { Id = id, Name = "Black suit111111", Photo = arr, Price = 33111.0, LastUpdated = DateTime.Now };

                var result = controller.Post(id, product).Result;

                if (result.GetType() == typeof(OkResult))
                    Assert.Pass();
                else
                {
                    var contentResult = (NegotiatedContentResult<string>)result;
                    Assert.Fail(contentResult.Content);
                }
            }
        }
    }
}
