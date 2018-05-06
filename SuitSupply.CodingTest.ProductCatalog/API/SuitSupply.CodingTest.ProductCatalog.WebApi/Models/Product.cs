using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers;
using SuitSupply.CodingTest.ProductCatalog.WebApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Models
{
    public class Product : IDisposable, IProduct
    {
        #region Constants
        //Logger
        private static readonly ILog _logger = LogManager.GetLogger(Environment.MachineName);
        //DB context
        private readonly ProductsContext _context = new ProductsContext(WebApiSettings.Instance.ProductsDbConnectionString); 
        #endregion

        #region Fields
        private int _id;
        private string _name;
        private byte[] _photo;
        private double _price;
        private DateTime _lastUpdated;
        #endregion

        #region Properties
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public byte[] Photo { get => _photo; set => _photo = value; }
        public double Price { get => _price; set => _price = value; }
        public DateTime LastUpdated { get => _lastUpdated; set => _lastUpdated = value; }
        #endregion

        #region Constructor
        public Product()
        {
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Gets all products as list
        /// </summary>
        /// <returns>List of products</returns>
        public List<IProduct> GetAllProducts()
        {
            try
            {
                return _context.Product.Select(p => p).ToList<IProduct>();
            }
            catch (DbEntityValidationException e)
            {
                _logger.Error(GetDbErrors(e));
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw e;
            }
        }
        /// <summary>
        /// Get single product by given id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Single product object</returns>
        public IProduct GetProductById(int id)
        {
            try
            {
                return _context.Product.SingleOrDefault(p => p.Id == id);
            }
            catch (DbEntityValidationException e)
            {
                _logger.Error(GetDbErrors(e));
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw e;
            }
        }
        /// <summary>
        /// Adds given product object to db context
        /// </summary>
        /// <param name="product">Product to be added</param>
        /// <returns>True if success</returns>
        public bool AddProduct(Product product)
        {
            try
            {
                using (var productDbTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Product.Add(product);
                        _context.SaveChanges();
                        _logger.Info($"Product with name {product.Name} added.");
                        productDbTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        productDbTransaction.Rollback();
                        return false;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                _logger.Error(GetDbErrors(e));
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw e;
            }
        }
        /// <summary>
        /// Gets product from id and edit with given object
        /// </summary>
        /// <param name="id">Product to be edited</param>
        /// <param name="product">Given product information</param>
        /// <returns>True if success</returns>
        public bool EditProduct(int id, Product product)
        {
            try
            {
                using (var productDbTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var selectedProduct = _context.Product.SingleOrDefault(p => p.Id == id);
                        if (selectedProduct == null)
                        {
                            throw new ArgumentNullException("selectedProduct", $"Selected product with id {id} not found.");
                        }
                        if (!CheckIfTwoProductsEqual(selectedProduct, product))
                        {
                            selectedProduct.Name = product.Name;
                            selectedProduct.Photo = product.Photo;
                            selectedProduct.Price = product.Price;
                            selectedProduct.LastUpdated = product.LastUpdated;
                            _context.SaveChanges();
                            _logger.Info($"Product with id {id} edited.");
                            productDbTransaction.Commit();
                        }
                        else
                            _logger.Warn($"Product {selectedProduct.Name} don't need to be updated.");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        productDbTransaction.Rollback();
                        return false;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                _logger.Error(GetDbErrors(e));
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw e;
            }
        }
        /// <summary>
        /// Removes given products from its id
        /// </summary>
        /// <param name="id">Given product id</param>
        /// <returns>True if success</returns>
        public bool RemoveProduct(int id)
        {
            try
            {
                using (var productDbTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var selectedProduct = _context.Product.SingleOrDefault(p => p.Id == id);
                        if (selectedProduct == null)
                        {
                            throw new ArgumentNullException("selectedProduct", $"Selected product with id {id} not found.");
                        }
                        _context.Product.Remove(selectedProduct);
                        _context.SaveChanges();
                        _logger.Info($"Product with name {selectedProduct.Name} deleted.");
                        productDbTransaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        productDbTransaction.Rollback();
                        return false;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                _logger.Error(GetDbErrors(e));
                throw e;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw e;
            }
        }
        #endregion

        #region Disposing context
        private bool disposed = false;
        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Collects database validation errors from exception
        /// </summary>
        /// <param name="e">Database validation exception</param>
        /// <returns>List of errors as text</returns>
        private static List<string> GetDbErrors(DbEntityValidationException e)
        {
            var outputLines = new List<string>();
            foreach (var eve in e.EntityValidationErrors)
            {
                outputLines.Add($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                foreach (var ve in eve.ValidationErrors)
                {
                    outputLines.Add($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }
            }

            return outputLines;
        }
        /// <summary>
        /// Checks if given two products are equal
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="incoming">Destination object</param>
        /// <returns>True if success</returns>
        private bool CheckIfTwoProductsEqual(Product source, Product incoming)
        {
            return source.Id == incoming.Id && source.Name == incoming.Name && source.Photo == incoming.Photo && source.Price == incoming.Price && source.LastUpdated == incoming.LastUpdated;
        }
        #endregion
    }
}