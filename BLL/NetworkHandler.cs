using System;
using System.Net.Http;
using System.Threading.Tasks;
using Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Identity;

namespace BLL
{
    public class NetworkHandler
    {
        private string API_KEY;

        private string keyVaultName;

        private readonly SerializationHandler serializer = new SerializationHandler();
        private static readonly HttpClient client = new HttpClient();

        public NetworkHandler()
        {
            keyVaultName = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUP__KEYVAULT__CONFIGURATIONVAULT");
            var client = new SecretClient(new Uri(keyVaultName), new DefaultAzureCredential());
            var secret =  client.GetSecret("fixerioapi");
            API_KEY = secret.Value.Value; 
        }

        private const string baseURI = "http://data.fixer.io/api/";

        public async Task<SuccessResponse> GetRates()
        {
            string uri = CreateRequest();
            var responseString = await SendRequest(uri);
            var response = serializer.DeSerializeJson(responseString);
            return response;
        }

        public async Task<SuccessResponse> GetRates(string FromCurrency, string ToCurrency)
        {
            string uri = CreateRequest(FromCurrency, ToCurrency, "latest");
            var responseString = await SendRequest(uri);
            var response = serializer.DeSerializeJson(responseString);
            return response;
        }

        public async Task<SuccessResponse> GetRates(string FromCurrency, string ToCurrency, string Date)
        {
            string uri = CreateRequest(FromCurrency, ToCurrency, Date);
            var responseString = await SendRequest(uri);
            var response = serializer.DeSerializeJson(responseString);
            return response;
        }

        public async Task<SuccessResponse> GetSymbols()
        {
            string uri = CreateSymbolRequest();
            var responseString = await SendRequest(uri);
            var response = serializer.DeSerializeJson(responseString);
            return response;
        }
        private string CreateRequest()
        {
            string fullUri = $"{baseURI}latest?access_key={API_KEY}";
            return fullUri;
        }

        private string CreateSymbolRequest()
        {
            string fullUri = $"{baseURI}symbols?access_key={API_KEY}";
            return fullUri;
        }

        private string CreateRequest(string FromCurrency, string ToCurrCurrency, string TimeFrame)
        {
            string fullUri = $"{baseURI}{TimeFrame}?access_key={API_KEY}&symbols={FromCurrency},{ToCurrCurrency}";
            return fullUri;
        }

        private async Task<string> SendRequest(string uri)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            try
            {
                HttpResponseMessage httpResponse = await client.GetAsync(uri);
                return await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"Httprequest was unsuccessfull.{e.Message}");
            }
        }
    }
}