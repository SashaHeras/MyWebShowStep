using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using MyWebShowStep.Data;
using System.IO;

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

        [HttpPost]
        public JsonResult GetProductImg() {
            var product = _context.Products.Where(p => p.Id == Convert.ToInt32(Request.Form["Id"].ToString())).FirstOrDefault();
            string fullPath = Path.Combine(path, product.ImagePath);
            product.Image = System.IO.File.ReadAllBytes(fullPath);

            return Json(product.Image);
        }
    }
}
