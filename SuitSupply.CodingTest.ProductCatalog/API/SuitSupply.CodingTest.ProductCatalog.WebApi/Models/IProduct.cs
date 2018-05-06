using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Models
{
    public interface IProduct
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        string Name { get; set; }
        byte[] Photo { get; set; }
        double Price { get; set; }
        DateTime LastUpdated { get; set; }
        #endregion

        #region Methods
        List<IProduct> GetAllProducts();
        IProduct GetProductById(int id);
        bool AddProduct(Product product);
        bool EditProduct(int id, Product product);
        bool RemoveProduct(int id);
        void Dispose();
        #endregion
    }
}