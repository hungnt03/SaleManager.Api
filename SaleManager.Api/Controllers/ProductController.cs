using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleManager.Api.Infrastructures;
using SaleManager.Api.Infrastructures.Entities;
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

        [HttpGet("getall")]
        public async Task<IActionResult> GetProducts()
        {
            var datas = await unitOfWork.ProductRepository.GetAll();
            AddImagePath(ref datas);
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
                    CategoryId = product.CategoryId,
                    SupplierId = product.SupplierId,
                });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] ConditionProductViewModel model)
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
                int totalPage = (int)Math.Ceiling((double)total / size);
                await foreach (var product in datas)
                {
                    products.Add(new ProductModel()
                    {
                        Barcode = product.Barcode,
                        Name = product.Name,
                        Price = product.Price,
                        ExpDate = product.ExpirationDate,
                        CategoryId = product.CategoryId,
                        SupplierId = product.SupplierId,
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
        public async Task<IActionResult> AddProduct([FromBody]AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await unitOfWork.ProductRepository.GetSingleByCondition(c => c.Barcode.Equals(model.Barcode));
                if (product != null)
                    return BadRequest(ModelState);

                if (product == null || string.IsNullOrEmpty(product.Barcode))
                {
                    Random rnd = new Random();
                    string barcode = string.Empty;
                    for (var i = 1; i <= 13; i++)
                        barcode += rnd.Next(0, 9).ToString();
                    model.Barcode = barcode;
                }

                product = new Product()
                {
                    Barcode = model.Barcode,
                    CategoryId = model.CategoryId,
                    Img = string.Empty,
                    CreatedBy = this.User.Identity.Name,
                    CreatedDate = DateTime.Now,
                    Enable = model.Enable,
                    ExpirationDate = model.ExpirationDate,
                    Name = model.Name,
                    Pin = model.Pin,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    SupplierId = model.SupplierId,
                    Unit = model.Unit,
                };
                var result = unitOfWork.ProductRepository.Add(product);
                unitOfWork.Commit();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult UpdateCategory([FromBody]UpdateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    CategoryId = model.CategoryId,
                    UpdatedBy = this.User.Identity.Name,
                    UpdatedDate = DateTime.Now,
                    Enable = model.Enable,
                    ExpirationDate = model.ExpirationDate,
                    Name = model.Name,
                    Pin = model.Pin,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    SupplierId = model.SupplierId,
                    Unit = model.Unit,
                };
                unitOfWork.ProductRepository.Update(product);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteCategory([FromBody]ProductSingleIdModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await unitOfWork.ProductRepository.GetSingleByCondition(c => c.Barcode.Equals(model.Barcode));
                if(product == null)
                    return BadRequest(ModelState);
                unitOfWork.ProductRepository.Delete(product);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }
        private void AddImagePath(ref IEnumerable<Product> products)
        {
            string currPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            foreach (var product in products)
                if (!string.IsNullOrEmpty(product.Img))
                    product.Img = currPath + product.Img;
        }
    }
}