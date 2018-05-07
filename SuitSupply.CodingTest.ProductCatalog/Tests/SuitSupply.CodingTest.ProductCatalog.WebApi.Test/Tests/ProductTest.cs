using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Drawing;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models;
using System.IO;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Test
{
    public class ProductTest
    {
        [TestFixture]
        public class Product_Add
        {
            List<Image> photos = null;
            IProduct productInterface = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                productInterface = new Product();
                foreach (var file in Directory.GetFiles(@"Z:\productcatalog"))
                {
                    photos.Add(Image.FromFile(file));
                }
            }

            [Test]
            public void Product_AddFromProduct_ShouldAddProductAndReturnTrue()
            {
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    Random rnd = new Random();
                    photos[rnd.Next(0, photos.Count - 1)].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }
                Product product = new Product { Name = "Green suit", Photo = arr, Price = 1.0, LastUpdated = DateTime.Now };
                Assert.AreEqual(true, productInterface.AddProduct(product));
            }
        }

        [TestFixture]
        public class Product_Get
        {
            List<Image> photos = null;
            IProduct productInterface = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                productInterface = new Product();
            }

            [Test]
            public void Product_GetAllProductsFromProduct_ShouldReturnAllProducts()
            {
                var productList = productInterface.GetAllProducts();
                Assert.NotNull(productList);
            }

            [TestCase(1)]
            [TestCase(3)]
            [TestCase(4)]
            [TestCase(8)]
            [TestCase(10)]
            public void GetProductByIdFromProduct_ShouldReturnProduct(int id)
            {
                var product = productInterface.GetProductById(id);
                Assert.NotNull(product);
            }
        }

        [TestFixture]
        public class Product_Remove
        {
            List<Image> photos = null;
            IProduct productInterface = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                productInterface = new Product();
            }

            [TestCase(3)]
            [TestCase(4)]
            public void Product_RemoveFromProduct_ShouldRemoveProductAndReturnTrue(int id)
            {
                var result = productInterface.RemoveProduct(id);
                Assert.AreEqual(true,result);
            }
        }

        [TestFixture]
        public class Product_Edit
        {
            List<Image> photos = null;
            IProduct productInterface = null;

            [SetUp]
            public void SetUp()
            {
                photos = new List<Image>();
                productInterface = new Product();
                foreach (var file in Directory.GetFiles(@"Z:\productcatalog"))
                {
                    photos.Add(Image.FromFile(file));
                }
            }

            [Test]
            public void Product_EditFromProduct_ShouldEditProductsPropertiesAndReturnTrue()
            {
                int id = 2;
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    Random rnd = new Random();
                    photos[rnd.Next(0, photos.Count - 1)].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }

                Product product = new Product { Id= id, Name = "Black suit", Photo = arr, Price = 33111.0, LastUpdated = DateTime.Now };
                var result = productInterface.EditProduct(id, product);

                Assert.AreEqual(true, result);
            }
        }
    }
}
