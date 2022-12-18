using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MyWebShowStep.Data;
using System;
using System.IO;
using System.Reflection.Metadata;

namespace MyWebShowStep.Controllers
{
    public class ProductController : Controller
    {
        private StepShopContext _context = new StepShopContext();
        private string path = "C:\\Users\\acsel\\source\\repos\\MyWebShowStep\\MyWebShowStep\\Pictures\\";

        [HttpGet]
        [Route("/Product/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTypes()
        {
            var types = _context.ProductTypes.ToList();

            return Json(types);
        }

        [HttpPost]
        [Route("Product/GetAllProducts")]
        public JsonResult GetAllProducts()
        {
            var _products = _context.Products;

            foreach ( var product in _products )
            {
                string fullPath = Path.Combine(path, product.ImagePath);
                product.Image = System.IO.File.ReadAllBytes(fullPath);
            }

            return Json(new { products = _products });
        }

        [HttpPost]
        public JsonResult GetProductsWithType()
        {
            int _type = Convert.ToInt32(Request.Form["Id"].ToString());
            var _products = _context.Products.Where(p => p.TypeId == _type);

            foreach (var product in _products)
            {
                string fullPath = Path.Combine(path, product.ImagePath);
                product.Image = System.IO.File.ReadAllBytes(fullPath);
            }

            return Json(new { products = _products });
        }
        
        [HttpGet]
        [Route("Product/GetProduct/{id?}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Where(p=>p.Id == id).FirstOrDefault();
            string fullPath = Path.Combine(path, product.ImagePath);
            product.Image = System.IO.File.ReadAllBytes(fullPath);

            ViewBag.Product = product;
            ViewBag.ProductType = _context.ProductTypes.Where(p => p.Id == product.TypeId).FirstOrDefault();
            object obj = null;

            if(product.TypeId == 1)
            {
                obj = _context.Gpus.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }
            else if(product.TypeId == 2)
            {
                obj = _context.Cpus.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }
            else if (product.TypeId == 3)
            {
                obj = _context.Monitors.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }

            ViewBag.ProductData = obj;

            return View();
        }

        [HttpGet]
        [Route("Product/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Where(p => p.Id == id).FirstOrDefault();

            ViewBag.Product = product;
            ViewBag.ProductType = _context.ProductTypes.Where(p => p.Id == product.TypeId).FirstOrDefault();
            object obj = null;

            if (product.TypeId == 1)
            {
                obj = _context.Gpus.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }
            else if (product.TypeId == 2)
            {
                obj = _context.Cpus.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }
            else if (product.TypeId == 3)
            {
                obj = _context.Monitors.Where(g => g.ProductId == product.Id).FirstOrDefault();
            }

            ViewBag.ProductData = obj;

            return View();
        }

        [HttpPost]
        public JsonResult Edit()
        {
            bool valid = true;

            Product p = new Product()
            {
                Id = Convert.ToInt32(Request.Form["productId"]),
                Name = Request.Form["prdName"].ToString(),
                ImagePath = Request.Form["imagePath"].ToString(),
                Price = Convert.ToInt32(Request.Form["prdPrice"]),
                TypeId = Convert.ToInt32(Request.Form["prdType"]),
                UserId = Convert.ToInt32(Request.Form["userId"])
            };

            if (Convert.ToInt32(Request.Form["prdType"]) == 1)
            {           
                Gpu gp = new Gpu()
                {
                    Id = Convert.ToInt32(Request.Form["id"]),
                    ProductId = Convert.ToInt32(Request.Form["productId"]),
                    Vendor = Request.Form["vendor"].ToString(),
                    GrapthChip = Request.Form["chip"].ToString(),
                    RAM = Convert.ToInt32(Request.Form["ram"]),
                    MemoryType = Request.Form["memory"].ToString()
                };

                _context.Gpus.Update(gp);
                _context.Products.Update(p);
                _context.SaveChanges();
            }

            return Json(valid);
        }

        [HttpPost]
        public JsonResult GetProductImg() {
            var product = _context.Products.Where(p => p.Id == Convert.ToInt32(Request.Form["Id"].ToString())).FirstOrDefault();
            string fullPath = Path.Combine(path, product.ImagePath);
            product.Image = System.IO.File.ReadAllBytes(fullPath);

            return Json(product.Image);
        }
    }
}
