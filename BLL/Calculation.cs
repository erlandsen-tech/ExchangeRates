using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Calculation
    {
        private NetworkHandler handler = new NetworkHandler();

        public async Task<string> FromToAmount(string FromCurrency, string ToCurrency, Decimal Amount, string date)
        {
            Decimal fromRate = 0;
            Decimal toRate = 0;
            string[] symbols = { FromCurrency, ToCurrency };
            var symbolValidation = await CheckSymbols(symbols);
            if (symbolValidation.Valid)
            {
                var rates = await handler.GetRates(FromCurrency, ToCurrency, date);
                rates.Rates.TryGetValue(FromCurrency, out fromRate);
                rates.Rates.TryGetValue(ToCurrency, out toRate);
                return Convert(fromRate, toRate, Amount).ToString();
            }
            else
            {
                string response = "Invalid symbols:";
                foreach (var symbol in symbolValidation.inValidSymbols)
                {
                    response += $"\n {symbol}";
                }
                return response;
            }
        }

        public async Task<Dictionary<string, string>> GetSymbols()
        {
            var response = await handler.GetSymbols();
            return response.Symbols;
        }
        private Decimal Convert(Decimal FromRate, Decimal ToRate, Decimal Amount)
        {
            try
            {
                Decimal total = (ToRate / FromRate) * Amount;
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception($"Calculation failed. {ex.Message}");
            }
        }
        private async Task<SymbolResponse> CheckSymbols(string[] symbols)
        {
            var SymbolResponse = new SymbolResponse();
            var validSymbols = await GetSymbols();
            var isSymbolValid = new Dictionary<string, bool>();
            bool valid = true;
            foreach (var key in symbols)
            {
                if (validSymbols.ContainsKey(key))
                {
                    isSymbolValid.Add(key, true);
                }
                else
                {
                    isSymbolValid.Add(key, false);
                    valid = false;
                }
            }
            if (!valid)
            {
                Console.WriteLine("One or more symbols where invalid. Use --symbols flag to get a list of valid ones");
                Console.WriteLine("These symbols where invalid");
                SymbolResponse.inValidSymbols = new List<string>();
                foreach (var key in isSymbolValid)
                {
                    if (!key.Value)
                    {
                        SymbolResponse.inValidSymbols.Add(key.Key);
                    }
                }
            }
            SymbolResponse.Valid = valid;
            return SymbolResponse;
        }
    }
}