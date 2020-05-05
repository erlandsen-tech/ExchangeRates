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
        private static readonly NetworkHandler handler = new NetworkHandler();
        private static readonly Calculation calc = new Calculation();
        [HttpGet]
        public async Task<SuccessResponse> GetAllRates()
        {
            return await handler.GetRates();
        }
        [HttpGet("{fromCurrency}/{toCurrency}/{amount}")]
        public async Task<CalculatedAmount> Convert(string fromCurrency, string toCurrency, Decimal amount)
        {
            string result = await calc.FromToAmount(fromCurrency.ToUpper(), toCurrency.ToUpper(), amount, "latest");
            var calcResult = new CalculatedAmount();
            calcResult.Amount = amount; calcResult.From = fromCurrency; calcResult.To = toCurrency; calcResult.Result = result;
            return calcResult;
        }
    }
}
