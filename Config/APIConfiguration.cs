using Microsoft.Extensions.Configuration;

namespace Shinsekai_API.Config
{
    public class ApiConfiguration
    {
        public IConfiguration Configuration { get; set; }

        public string ConnectionString { get; set; }
        public string JwtSecretKey { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string StripeKey { get; set; }
        public string MailServiceEmail { get; set; }

        public string MailServicePassword { get; set; }
        public string SendGridApiKey { get; set; }
        public ApiConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
            MapValues();
        }

        private void MapValues()
        {
            ConnectionString = Configuration["DbConnectionString"];
            JwtSecretKey = Configuration["JwtKey"];
            StripeKey = Configuration["StripeKey"];
            MailServiceEmail = Configuration["SenderEmail"];
            MailServicePassword = Configuration["MailPassword"];
            BlobStorageConnectionString = Configuration["StorageConnectionString"];
            SendGridApiKey = Configuration["SendGridApiKey"];
        }
    }
}