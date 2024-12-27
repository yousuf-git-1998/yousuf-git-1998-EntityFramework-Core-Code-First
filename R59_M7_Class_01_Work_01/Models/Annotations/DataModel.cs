using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace R59_M7_Class_01_Work_01.Models
{
    [MetadataType(typeof(ProductData))]
    public partial class Product
    {
    }
    [MetadataType(typeof(SaleData))]
    public partial class Sale
    {

    }
    public class ProductData
    {
        public int ProductId { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required, DataType(DataType.Date)]
        public System.DateTime MfgDate { get; set; }
        public Nullable<bool> InStock { get; set; }
        [Required, StringLength(50)]
        public string Picture { get; set; }
    }
    public class SaleData
    {
        public int SalesId { get; set; }
        [Required, DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        [Required]
        public int SellerId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}