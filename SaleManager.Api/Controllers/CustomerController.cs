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
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        public CustomerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var datas = await unitOfWork.CustomerRepository.GetAll();
            return Ok(datas);
        }

        [Authorize]
        [HttpGet("getbyid/{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = unitOfWork.CustomerRepository.GetSingleById(id);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddCategory([FromBody]CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Name = model.Name,
                    Contact = model.Contact,
                    Address = model.Address,
                    CreatedBy = this.User.Identity.Name,
                    CreatedDate = DateTime.Now
                };
                var result = unitOfWork.CustomerRepository.Add(customer);
                unitOfWork.Commit();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult UpdateCategory([FromBody]CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                    Contact = model.Contact,
                    UpdatedBy = this.User.Identity.Name,
                    UpdatedDate = DateTime.Now
                };
                unitOfWork.CustomerRepository.Update(customer);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPost("delete")]
        public IActionResult DeleteCategory([FromBody]CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CustomerRepository.Delete(model.Id);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}