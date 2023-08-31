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
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet("/Products")]
        [SwaggerOperation(Summary = "Get products", Description = "Retrieve a list of products.")]
        [SwaggerResponse(200, "List of products retrieved successfully.")]
        public async Task<IActionResult> Index(int? page)
        {            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            IQueryable<Product> iseriq = from p in _context.Product.Where(p => p.SupplierId == userId)
                                             select p;


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await iseriq.ToPagedListAsync(pageNumber, pageSize));


        }







        // post: Products1
        [HttpPost("/Products/ImportFromExcel")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Import products from Excel", Description = "Import products from an Excel file.")]
        [SwaggerResponse(200, "Products imported successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
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
        [HttpGet("/Products/Details/{id}")]
        [SwaggerOperation(Summary = "Get details of a product", Description = "Retrieve the details of a product.")]
        [SwaggerResponse(200, "Product details retrieved successfully.")]
        [SwaggerResponse(404, "Product not found.")]
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
        [HttpGet("/Products/Create")]
        [SwaggerOperation(Summary = "Show product creation form", Description = "Display the product creation form.")]
        [SwaggerResponse(200, "Product creation form displayed successfully.")]
        public IActionResult Create()
        {
            return View();
        }




        // POST: Products1/Create
        [HttpPost("/Products/Create")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Create a new product", Description = "Create a new product.")]
        [SwaggerResponse(200, "Product created successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
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
        [HttpGet("/Products/Edit/{id}")]
        [SwaggerOperation(Summary = "Show product edit form", Description = "Display the product edit form.")]
        [SwaggerResponse(200, "Product edit form displayed successfully.")]
        [SwaggerResponse(404, "Product not found.")]
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
        [HttpPost("/Products/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Edit a product", Description = "Edit an existing product.")]
        [SwaggerResponse(200, "Product edited successfully.")]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Product not found.")]
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
        [HttpGet("/Products/Delete/{id}")]
        [SwaggerOperation(Summary = "Show product delete confirmation", Description = "Display the product delete confirmation.")]
        [SwaggerResponse(200, "Product delete confirmation displayed successfully.")]
        [SwaggerResponse(404, "Product not found.")]
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
        [HttpPost("/Products/DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        [SwaggerOperation(Summary = "Delete a product", Description = "Delete an existing product.")]
        [SwaggerResponse(200, "Product deleted successfully.")]
        [SwaggerResponse(404, "Product not found.")]
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









        /*---------------------------------------------------------------*/






        // Helper: no route
        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }



    }
}
