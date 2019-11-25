using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleManager.Api.Infrastructures;
using SaleManager.Api.Models.Product;

namespace SaleManager.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var datas = await unitOfWork.ProductRepository.GetAll();
            return Ok(datas.OrderBy(r => r.Name));
        }

        [HttpPost("product")]
        public async Task<IActionResult> Product([FromBody] ProductSingleIdModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await unitOfWork.ProductRepository.GetSingleByCondition(c=>c.Barcode.Equals(model.Barcode));
                var categories = await unitOfWork.CategoryRepository.GetAll();
                var suppliers = await unitOfWork.SupplierRepository.GetAll();
                return Ok(new ProductModel()
                {
                    Barcode = product.Barcode,
                    Name = product.Name,
                    Price = product.Price,
                    ExpDate = product.ExpirationDate,
                    Category = categories.Where(c => c.Id == product.CategoryId).Select(s => s.Name).FirstOrDefault(),
                    Supplier = suppliers.Where(c => c.Id == product.SupplierId).Select(s => s.Name).FirstOrDefault(),
                });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductConditionModel model)
        {
            int total = 0;
            int index = 0;
            int size = 20;
            var products = new List<ProductModel>();
            if (ModelState.IsValid)
            {
                var datas = unitOfWork.ProductRepository.GetMultiPaging(r =>
                (!string.IsNullOrEmpty(model.NameOrBarcode) && r.Name.Contains(model.NameOrBarcode)) ||
                (model.Category != 0 && r.CategoryId == model.Category) ||
                (model.Supplier != 0 && r.SupplierId == model.Supplier), out total, index, size);
                var categories = await unitOfWork.CategoryRepository.GetAll();
                var suppliers = await unitOfWork.SupplierRepository.GetAll();
                int totalPage = (int)Math.Ceiling((double)total / size);
                await foreach (var product in datas)
                {
                    products.Add(new ProductModel()
                    {
                        Barcode = product.Barcode,
                        Name = product.Name,
                        Price = product.Price,
                        ExpDate = product.ExpirationDate,
                        Category = categories.Where(c => c.Id == product.CategoryId).Select(s => s.Name).FirstOrDefault(),
                        Supplier = suppliers.Where(c => c.Id == product.SupplierId).Select(s => s.Name).FirstOrDefault(),
                    });
                }
                return Ok(new PaginationSet<ProductModel>()
                {
                    Items = products,
                    MaxPage = total,
                    Page = index,
                    TotalCount = total,
                    TotalPages = totalPage
                });
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddProduct([FromBody]ProductAddModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var category = new Category()
            //    {
            //        Name = model.Name,
            //        Description = model.Description,
            //        CreatedBy = this.User.Identity.Name,
            //        CreatedDate = DateTime.Now
            //    };
            //    var result = unitOfWork.CategoryRepository.Add(category);
            //    unitOfWork.Commit();
            //    return Ok(result);
            //}
            return BadRequest(ModelState);
        }
    }
}