using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyWebShowStep.Data;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

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
            int? _type = Convert.ToInt32(Request.Form["Id"].ToString());            
            List<Product> _products = _context.Products.ToList();
            _products = SortByDefaultFilters(_products, Request);

            if (_type != null && _type != 0)
            {
                if (_type == 1)
                {
                    var filters = _context.Filters.Where(f => f.ProductTypeId == _type).ToList();
                    List<string> filtersString = new List<string>();

                    foreach(var f in filters)
                    {
                        filtersString.Add(Request.Form[f.HtmlId].ToString());
                    }

                    _products = _products.Where(p=>p.TypeId == _type).ToList();

                    if (filtersString[0] != "")
                    {
                        _products = (from prd in _products
                                    where (
                                    from gpu in _context.Gpus
                                    where gpu.Vendor.Contains(filtersString[0]) == true
                                    select gpu.ProductId
                                    ).Contains(prd.Id) select prd).ToList();
                    }
                    if (filtersString[1] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Gpus
                                     where gpu.GrapthChip.Contains(filtersString[1]) == true
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[2] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Gpus
                                     where gpu.RAM == Convert.ToInt32(filtersString[2])
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[3] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Gpus
                                     where gpu.MemoryType.Contains(filtersString[3]) == true
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                }
                else if (_type == 2)
                {
                    var filters = _context.Filters.Where(f => f.ProductTypeId == _type).ToList();
                    List<string> filtersString = new List<string>();

                    foreach (var f in filters)
                    {
                        filtersString.Add(Request.Form[f.HtmlId].ToString());
                    }

                    _products = _context.Products.Where(p => p.TypeId == _type).ToList();

                    if (filtersString[0] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Cpus
                                     where gpu.CoreFamily.Contains(filtersString[0]) == true
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[1] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Cpus
                                     where gpu.Speed == Convert.ToInt32(filtersString[1])
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[2] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Cpus
                                     where gpu.CoresCount == Convert.ToInt32(filtersString[2])
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                }
                else if (_type == 3)
                {
                    var filters = _context.Filters.Where(f => f.ProductTypeId == _type).ToList();
                    List<string> filtersString = new List<string>();

                    foreach (var f in filters)
                    {
                        filtersString.Add(Request.Form[f.HtmlId].ToString());
                    }

                    _products = _context.Products.Where(p => p.TypeId == _type).ToList();

                    if (filtersString[0] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Monitors
                                     where gpu.DisplaySize.Contains(filtersString[0]) == true
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[1] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Monitors
                                     where gpu.RefreshSpeed == Convert.ToInt32(filtersString[1])
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                    if (filtersString[2] != "")
                    {
                        _products = (from prd in _products
                                     where (
                                     from gpu in _context.Monitors
                                     where gpu.MatrixType.Contains(filtersString[2]) == true
                                     select gpu.ProductId
                                     ).Contains(prd.Id)
                                     select prd).ToList();
                    }
                }
            }
            else if(_type == 0)
            {

            }
            else
            {
                _products = _context.Products.Where(p => p.TypeId == _type).ToList();
            }

            foreach (var product in _products)
            {
                string fullPath = Path.Combine(path, product.ImagePath);
                product.Image = System.IO.File.ReadAllBytes(fullPath);
            }

            return Json(new { products = _products });
        }

        public List<Product> SortByDefaultFilters(List<Product> products, HttpRequest req)
        {
            int _priceFrom = Request.Form["priceFrom"] != "" ? Convert.ToInt32(Request.Form["priceFrom"].ToString()) : -1;
            int _priceTo = Request.Form["priceTo"] != "" ? Convert.ToInt32(Request.Form["priceTo"].ToString()) : -1;
            string _prdName = Request.Form["prdName"] != "" ? Request.Form["prdName"].ToString() : "";

            if (_prdName != "")
            {
                products = products.Where(p => p.Name.Contains(_prdName) == true).ToList();
            }
            if (_priceFrom != -1)
            {
                products = (from prd in products
                             where prd.Price >= _priceFrom
                             select prd).ToList();
            }
            if (_priceTo != -1)
            {
                products = (from prd in products
                             where prd.Price <= _priceTo
                             select prd).ToList();
            }

            return products;
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

        [HttpGet]
        [Route("Product/DeleteProduct/{id?}")]
        public IActionResult DeleteProduct(int id)
        {
            Product prd = _context.Products.Where(p => p.Id == id).FirstOrDefault();
            var filePath = "C:/Users/acsel/source/repos/MyWebShowStep/MyWebShowStep/Pictures/" + prd.ImagePath;
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            if (prd.TypeId == 1)
            {
                Gpu gpu = _context.Gpus.Where(g => g.ProductId == prd.Id).FirstOrDefault();
                _context.Gpus.Remove(gpu);
                _context.Products.Remove(prd);
                _context.SaveChanges();
            }
            if (prd.TypeId == 2)
            {
                Cpu cpu = _context.Cpus.Where(c => c.ProductId == prd.Id).FirstOrDefault();
                _context.Cpus.Remove(cpu);
                _context.Products.Remove(prd);
                _context.SaveChanges();
            }
            if (prd.TypeId == 3)
            {
                Data.Monitor mon = _context.Monitors.Where(m => m.ProductId == prd.Id).FirstOrDefault();
                _context.Monitors.Remove(mon);
                _context.Products.Remove(prd);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Product");
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
            else if (Convert.ToInt32(Request.Form["prdType"]) == 2)
            {
                Cpu cp = new Cpu()
                {
                    Id = Convert.ToInt32(Request.Form["id"]),
                    ProductId = Convert.ToInt32(Request.Form["productId"]),
                    CoreFamily = Request.Form["coreFamily"].ToString(),
                    CoresCount = Convert.ToInt32(Request.Form["coreCount"]),
                    Speed = Convert.ToInt32(Request.Form["speed"])
                };

                _context.Cpus.Update(cp);
                _context.Products.Update(p);
                _context.SaveChanges();
            }
            else if (Convert.ToInt32(Request.Form["prdType"]) == 3)
            {
                Data.Monitor mp = new Data.Monitor()
                {
                    Id = Convert.ToInt32(Request.Form["id"]),
                    ProductId = Convert.ToInt32(Request.Form["productId"]),
                    DisplaySize = Request.Form["displaySize"].ToString(),
                    MatrixType = Request.Form["matrixType"].ToString(),
                    RefreshSpeed = Convert.ToInt32(Request.Form["refreshSpeed"])
                };

                _context.Monitors.Update(mp);
                _context.Products.Update(p);
                _context.SaveChanges();
            }

            return Json(valid);
        }

        [HttpPost]
        public JsonResult GetFilters()
        {
            int tId = Convert.ToInt32(Request.Form["id"]);

            var filters = _context.Filters.Where(f => f.ProductTypeId == 0).ToList();
            if(tId != 0)
            {
                filters.AddRange(_context.Filters.Where(f => f.ProductTypeId == tId).ToList());
            }

            return Json(filters);
        }

        [HttpPost]
        public JsonResult GetProductImg() {
            var product = _context.Products.Where(p => p.Id == Convert.ToInt32(Request.Form["Id"].ToString())).FirstOrDefault();
            string fullPath = Path.Combine(path, product.ImagePath);
            product.Image = System.IO.File.ReadAllBytes(fullPath);

            return Json(product.Image);
        }

        [HttpGet]
        [Route("Product/CreateProduct")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCreateFields()
        {
            int tId = Convert.ToInt32(Request.Form["id"]);

            var fields = _context.CreatePrdFields.Where(f => f.ProductTypeId == tId).ToList();

            return Json(fields);
        }

        [HttpPost]
        public JsonResult GetAllProductTypes()
        {
            var types = _context.ProductTypes;

            return Json(types);
        }

        [HttpPost]
        public JsonResult Create()
        {
            int type = Convert.ToInt32(Request.Form["prdType"]);

            var file = Request.Form.Files[0];
            var path = "C:\\Users\\acsel\\source\\repos\\MyWebShowStep\\MyWebShowStep\\Pictures\\";
            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(path, fileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }

            Product prd = new Product()
            {
                UserId = 1,
                Name = Request.Form["prdName"].ToString(),
                TypeId = type,
                Price = Convert.ToInt32(Request.Form["prdPrice"]),
                ImagePath = fileName
            };

            _context.Products.Add(prd);
            _context.SaveChanges();

            if (type == 1)
            {
                Gpu gp = new Gpu()
                {
                    Vendor = Request.Form["Vendor"].ToString(),
                    GrapthChip = Request.Form["GrapthChip"].ToString(),
                    RAM = Convert.ToInt32(Request.Form["RAM"]),
                    MemoryType = Request.Form["MemoryType"].ToString(),
                    ProductId = prd.Id
                };

                _context.Gpus.Add(gp);
                _context.SaveChanges();
            }
            if (type == 2)
            {
                Cpu cp = new Cpu()
                {
                    CoreFamily = Request.Form["CoreFamily"].ToString(),
                    CoresCount = Convert.ToInt32(Request.Form["CoresCount"]),
                    Speed = Convert.ToInt32(Request.Form["Speed"]),
                    ProductId = prd.Id
                };

                _context.Cpus.Add(cp);
                _context.SaveChanges();
            }
            if (type == 3)
            {
                Data.Monitor mp = new Data.Monitor()
                {
                    DisplaySize = Request.Form["DisplaySize"].ToString(),
                    MatrixType = Request.Form["MatrixType"].ToString(),
                    RefreshSpeed = Convert.ToInt32(Request.Form["RefreshSpeed"]),
                    ProductId = prd.Id
                };

                _context.Monitors.Add(mp);
                _context.SaveChanges();
            }

            return Json(true);
        }
    }
}
