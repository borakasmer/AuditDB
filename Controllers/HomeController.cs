using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using auditDB.Models;

namespace auditDB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (ProductContext dbContext = new ProductContext())
            {
                var data = dbContext.Products.ToList();
                return View(data);
            }
        }
        public IActionResult Detail(int? ID)
        {
            Product product = null;
            if (ID == null)
            {
                product = new Product();
            }
            else
            {
                using (ProductContext dbContext = new ProductContext())
                {
                    product = dbContext.Products.FirstOrDefault(prod => prod.ID == ID);
                }
            }
            return View(product);
        }
        public async Task<IActionResult> Update(Product product)
        {
            using (ProductContext dbContext = new ProductContext())
            {             
                if (product.ID == 0)
                {
                    var newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.SerieNo = product.SerieNo;
                    newProduct.Price = product.Price;
                    newProduct.TotalCount = product.TotalCount;
                    newProduct.WarehouseAddress = product.WarehouseAddress;   
                    await dbContext.Products.AddAsync(newProduct);
                }
                else
                {
                    var updateProduct = dbContext.Products.FirstOrDefault(prod => prod.ID == product.ID);
                    updateProduct.Name = product.Name;
                    updateProduct.SerieNo = product.SerieNo;
                    updateProduct.Price = product.Price;
                    updateProduct.TotalCount = product.TotalCount;
                    updateProduct.WarehouseAddress = product.WarehouseAddress;
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
