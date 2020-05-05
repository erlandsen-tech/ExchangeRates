using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class Calculation
    {
        private NetworkHandler handler = new NetworkHandler();

        public async Task<Decimal> FromToAmount(string FromCurrency, string ToCurrency, Decimal Amount, string date)
        {
            Decimal fromRate = 0;
            Decimal toRate = 0;
            string[] symbols = { FromCurrency, ToCurrency };
            if (await CheckSymbols(symbols))
            {
                var rates = await handler.GetRates(FromCurrency, ToCurrency, date);
                rates.Rates.TryGetValue(FromCurrency, out fromRate);
                rates.Rates.TryGetValue(ToCurrency, out toRate);
                return Convert(fromRate, toRate, Amount);
            }
            else
            {
                throw new Exception("Unable to validate symbols");
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


        private async Task<bool> CheckSymbols(string[] symbols)
        {
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
                foreach (var key in isSymbolValid)
                {
                    if (!key.Value)
                    {
                        Console.WriteLine(key.Key.ToString());
                    }
                }
            }
            return valid;
        }
    }
}