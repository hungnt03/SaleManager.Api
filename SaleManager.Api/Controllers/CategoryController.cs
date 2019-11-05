using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleManager.Api.Infrastructures;
using SaleManager.Api.Infrastructures.Entities;
using SaleManager.Api.Models;

namespace SaleManager.Api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var datas = await unitOfWork.CategoryRepository.GetAll();
            return Ok(datas);
        }

        [Authorize]
        [HttpGet("getbyid/{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = unitOfWork.CategoryRepository.GetSingleById(id);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddCategory([FromBody]CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatedBy = this.User.Identity.Name,
                    CreatedDate = DateTime.Now
                };
                var result = unitOfWork.CategoryRepository.Add(category);
                unitOfWork.Commit();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult UpdateCategory([FromBody]CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    UpdatedBy = this.User.Identity.Name,
                    UpdatedDate = DateTime.Now
                };
                unitOfWork.CategoryRepository.Update(category);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPost("delete")]
        public IActionResult DeleteCategory([FromBody]int id)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Delete(id);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }

    }
}