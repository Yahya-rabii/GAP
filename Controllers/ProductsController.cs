using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GAP.Data;
using GAP.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using X.PagedList;
using OfficeOpenXml;

namespace GAP.Controllers
{

    [Authorize]
    [Authorize(Roles = "Supplier , Admin")]
    public class ProductsController : Controller
    {
        private readonly GAPContext _context;

        public ProductsController(GAPContext context)
        {
            _context = context;
        }

        // GET: Products1
        public async Task<IActionResult> Index(int? page)
        {            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<Product> iseriq = from p in _context.Product.Where(p => p.SupplierId == userId)
                                             select p;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));


        }








        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(IFormFile excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial

            if (excelFile == null || excelFile.Length <= 0)
            {
                // Handle error: No file uploaded
                return RedirectToAction("Index");
            }

            List<string> duplicateProducts = new List<string>(); // Initialize as an empty list

            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                List<Product> newProducts = new List<Product>();

                for (int row = 2; row <= rowCount; row++)
                {
                    string name = worksheet.Cells[row, 1].Value?.ToString();

                    if (string.IsNullOrEmpty(name))
                    {
                        // Skip empty rows
                        continue;
                    }

                    string description = worksheet.Cells[row, 4].Value?.ToString();

                    if (_context.Product.Any(p => p.Name == name && p.Desc == description))
                    {
                        // Handle duplicate products
                        duplicateProducts.Add(name);
                        continue;
                    }

                    string unitPriceString = worksheet.Cells[row, 2].Value?.ToString();
                    if (!float.TryParse(unitPriceString, out float unitPrice))
                    {
                        // Handle invalid unit price if needed
                        continue;
                    }

                    string itemsNumberString = worksheet.Cells[row, 3].Value?.ToString();
                    if (!int.TryParse(itemsNumberString, out int itemsNumber))
                    {
                        // Handle invalid items number if needed
                        continue;
                    }

                    // Rest of the code to create a new Product instance...
                    var newProduct = new Product
                    {
                        Name = name,
                        Unitprice = unitPrice,
                        ItemsNumber = itemsNumber,
                        Desc = description,
                    };

                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    newProduct.SupplierId = userId;

                    newProducts.Add(newProduct);
                }

                _context.Product.AddRange(newProducts);
                await _context.SaveChangesAsync();
            }

            if (duplicateProducts.Any())
            {
                TempData["DuplicateProducts"] = duplicateProducts;
            }

            return RedirectToAction("Index");
        }














        // GET: Products1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var Product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: Products1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,Unitprice,Name,ItemsNumber,Desc,ProductPicture")] Product product, IFormFile productPicture)
        {
            if (ModelState.IsValid)
            {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                product.SupplierId = userId;
                product.Totalprice = (float)(product.ItemsNumber * product.Unitprice);

                if (productPicture != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await productPicture.CopyToAsync(memoryStream);
                        product.ProductPicture = memoryStream.ToArray();
                    }
                }


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var Product = await _context.Product.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        // POST: Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Unitprice,CompanyName,ItemsNumber,Desc")] Product Product)
        {
            if (id != Product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    Product.SupplierId = userId;
                    Product.Totalprice = (float)(Product.Unitprice * Product.ItemsNumber);

                    _context.Update(Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        } 
        
        
      

        // GET: Products1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var Product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'GAPContext.Product'  is null.");
            }
            var Product = await _context.Product.FindAsync(id);
            if (Product != null)
            {
                _context.Product.Remove(Product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
