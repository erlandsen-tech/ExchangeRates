using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class CurrencyDBContext : DbContext
    {
        private string keyVaultName;
        private string Connection_string;
        public CurrencyDBContext()
        {

            keyVaultName = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUP__KEYVAULT__CONFIGURATIONVAULT");
            var client = new SecretClient(new Uri(keyVaultName), new DefaultAzureCredential());
            var secret =  client.GetSecret("ConnectionStrings--scheduledFetchDb");
            Connection_string = secret.Value.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection_string);
        }

        public DbSet<ResponseDBModel> Responses { get; set; }
    }
}
