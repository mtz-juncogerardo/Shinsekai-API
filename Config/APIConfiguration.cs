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
        public ApiConfiguration()
        {
            MapValues();
        }

        private void MapValues()
        {
            ConnectionString = "Server=tcp:shinsekai.database.windows.net,1433;Initial Catalog=Shinsekai-DB;Persist Security Info=False;User ID=shinsekai;Password=garabato33!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            JwtSecretKey = "wWLk~]bB5UXY-dH8c,geFws!+<_?#5*($Du3~2K>+;AC+h:yhzCReW<2Y)&r[&3";
            StripeKey = "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";
            MailServiceEmail = "shinsekai.ml@hotmail.com";
            MailServicePassword = "Dirithcm6";
            BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=shinsekaistorage;AccountKey=272nTm6OJChvjj2dD3egakMiiQNKajcgAJnbwdSdDcnrV8InKQ2Svtpox9dWDrG02xeCFIQ6l8PjAR7zsMQg/g==;EndpointSuffix=core.windows.net";
            SendGridApiKey = "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";
        }
    }
}