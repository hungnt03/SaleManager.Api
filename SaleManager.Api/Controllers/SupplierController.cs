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
    [Route("api/supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        public SupplierController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var datas = await unitOfWork.SupplierRepository.GetAll();
            return Ok(datas);
        }

        [Authorize]
        [HttpGet("getbyid/{id:int}")]
        public IActionResult GetById(int id)
        {
            var data = unitOfWork.SupplierRepository.GetSingleById(id);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddSupplier([FromBody]SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Supplier()
                {
                    Address = model.Address,
                    Contact1 = model.Contact1,
                    Contact2 = model.Contact2,
                    Name = model.Name,
                    CreatedBy = this.User.Identity.Name,
                    CreatedDate = DateTime.Now
                };
                var result = unitOfWork.SupplierRepository.Add(supplier);
                unitOfWork.Commit();
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("update")]
        public IActionResult UpdateSupplier([FromBody]SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Supplier()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Address = model.Address,
                    Contact1 = model.Contact1,
                    Contact2 = model.Contact2,
                    UpdatedBy = this.User.Identity.Name,
                    UpdatedDate = DateTime.Now
                };
                unitOfWork.SupplierRepository.Update(supplier);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("delete")]
        public IActionResult DeleteSupplier([FromBody]SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.SupplierRepository.Delete(model.Id);
                unitOfWork.Commit();
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}