using R59_M7_Class_01_Work_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace R59_M7_Class_01_Work_01.ViewModels
{
    public class ProductInsertModel
    {
        public int ProductId { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required, DataType(DataType.Date)]
        public System.DateTime MfgDate { get; set; }
        public bool InStock { get; set; }
        [Required]
        public HttpPostedFileBase Picture { get; set; }
        public List<Sale> Sales { get; set; }= new List<Sale>();
    }
}