using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleManager.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaleManager.Api.Controllers
{
    public class BaseAiController: ControllerBase
    {
        public ApplicationUser currentUser;
        public BaseAiController(IHttpContextAccessor httpContextAccessor)
        {
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User
        }

    }
}
