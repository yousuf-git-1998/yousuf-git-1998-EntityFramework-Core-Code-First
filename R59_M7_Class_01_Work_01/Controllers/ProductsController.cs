using R59_M7_Class_01_Work_01.Models;
using R59_M7_Class_01_Work_01.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace R59_M7_Class_01_Work_01.Controllers
{
    public class ProductsController : Controller
    {
        readonly M7C1DbContext db = new M7C1DbContext();
        // GET: Products
        public ActionResult Index(int pg = 1)
        {
            ViewBag.TotalPages = (int)Math.Ceiling(db.Products.Count() / 5.0);
            ViewBag.CurrentPage = pg;
            var data = db.Products.OrderBy(x => x.ProductId).Skip((pg - 1) * 5).Take(5);
            return View(data);
        }
        public ActionResult Create()
        {
            var model = new ProductInsertModel();
            model.Sales.Add(new Sale { });
            ViewBag.Sellers = db.Sellers.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProductInsertModel model, string operation)
        {
            if (operation == "add")
            {
                model.Sales.Add(new Sale { });
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (operation.StartsWith("del"))
            {
                int pos = operation.IndexOf("_");
                int index = int.Parse(operation.Substring(pos + 1));
                model.Sales.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (operation == "insert")
            {
                if (ModelState.IsValid)
                {
                    var p = new Product
                    {
                        ProductName = model.ProductName,
                        Price = model.Price,
                        MfgDate = model.MfgDate,
                        InStock = model.InStock
                    };
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                    model.Picture.SaveAs(savePath);
                    p.Picture = f;
                    foreach (var s in model.Sales)
                    {
                        p.Sales.Add(new Sale { Date=s.Date, SellerId=s.SellerId, Quantity=s.Quantity});
                    }
                    db.Products.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Sellers = db.Sellers.ToList();
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var product = db.Products.FirstOrDefault(p=> p.ProductId == id);
            if(product == null) return new HttpNotFoundResult();
            var model = new ProductEditModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                MfgDate = product.MfgDate,
                InStock = product.InStock.Value,
                
            };
            model.Sales = product.Sales.ToList();
            ViewBag.CurrentPic = product.Picture;
            ViewBag.Sellers = db.Sellers.ToList();
            return View(model);
        }
        [HttpPost]

        public ActionResult Edit(ProductEditModel model, string operation)
        {
            var p = db.Products.FirstOrDefault(x=> x.ProductId ==model.ProductId);
            if(p== null) return new HttpNotFoundResult();
            if (operation == "add")
            {
                model.Sales.Add(new Sale { Date=DateTime.Now});
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (operation.StartsWith("del"))
            {
                int pos = operation.IndexOf("_");
                int index = int.Parse(operation.Substring(pos + 1));
                model.Sales.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (operation == "update")
            {
                if (ModelState.IsValid)
                {
                    p.ProductName = model.ProductName;
                    p.Price = model.Price;
                    p.MfgDate = model.MfgDate;
                    p.InStock = model.InStock;

                   
                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                        model.Picture.SaveAs(savePath);
                        p.Picture = f;
                    }
                    db.Database.ExecuteSqlCommand($"DELETE  Sales WHERE productid={p.ProductId}");
                    foreach (var s in model.Sales)
                    {
                        db.Sales.Add(new Sale { Date = s.Date, SellerId = s.SellerId, Quantity = s.Quantity, ProductId=p.ProductId });
                    }
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Sellers = db.Sellers.ToList();
            ViewBag.CurrentPic = p.Picture;
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var p = db.Products.FirstOrDefault(x => x.ProductId == id);
            if (p == null) return new HttpNotFoundResult();
            return View(p);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DoDelete(int id)
        {
            var p = db.Products.FirstOrDefault(x => x.ProductId == id);
            if (p == null) return new HttpNotFoundResult();
            db.Sales.RemoveRange(p.Sales.ToList());
            db.Products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
    
}