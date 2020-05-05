using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Model;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        NetworkHandler handler;
        [HttpGet]
        public async Task<SuccessResponse> GetAllRates()
        {
            handler = new NetworkHandler();
            return await handler.GetRates();
        }
    }
}
