using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleManager.Api.Infrastructures;
using SaleManager.Api.Models.Account;

namespace SaleManager.Api.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var results = new List<RoleModelView>();
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null || roles.Count == 0)
                return NotFound(new ResponseData(ModelState));

            foreach(var role in roles)
            {
                results.Add(new RoleModelView() { Id= role.Id, Role=role.Name });
            }

            return Ok(new ResponseData(results));
        }

        //[Authorize(Roles = "admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody]RoleModelView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseData(ModelState));

            var role = await _roleManager.FindByNameAsync(model.Role);
            if (role == null)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                if (result.Succeeded)
                {
                    return Ok(new ResponseData(result));
                }
                AddErrors(result);
            }
            return BadRequest(new ResponseData(ModelState));
        }

        //[Authorize(Roles = "admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRole([FromBody]RoleModelView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseData(ModelState));

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new ResponseData(result));
                }
                AddErrors(result);
            }
            return BadRequest(new ResponseData(ModelState));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}