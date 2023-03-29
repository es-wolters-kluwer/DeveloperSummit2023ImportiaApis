using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace WkeHandsOnImportia.Pages.Importia
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostGenerateIntegrationFile()
        {

            var migrationSet = ImportiaMigrationSetBuilder.CreateMigrationSet();
            var migrationSetZipFile = new ImportiaZipBuilder().CreateMigrationSetZipFile(migrationSet);

            return File(migrationSetZipFile, "application/octet-stream", $"{migrationSet.Info.FileName}.zip");

        }

        [BindProperty]
        public IFormFile IntegrationFile { get; set; }

        public async Task OnPostUploadFile()
        {

            using (var stream = new MemoryStream())
            {
                await IntegrationFile.CopyToAsync(stream);

                var baseUrl = $"https://a3api.wolterskluwer.es/a3innuva-contabilidad-importia/";

                var companyCorrelationId = $"27200f28-b59c-4f63-aa89-7fabcf16f96f";
                var activityCorrelationId = $"62fd87e8-4050-4bf3-bab1-8c02f1e9f716";
                var channelCorrelationId = $"819cc23f-d62b-4336-b98c-37fce9fbcd75";

                var accessToken = $"bearer {User.FindFirstValue("access_token")}";
                var context = "{ \"clientId\": \"" + companyCorrelationId + "\" }";
                var a3SubscriptionKey = $"c65da85d8b254b25931066bfd300edfa";

                using (var httpClient = new HttpClient())
                {

                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"{accessToken}");
                    httpClient.DefaultRequestHeaders.Add("context", $"{context}");
                    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", a3SubscriptionKey);
                    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");

                    using (var multiFormContent = new MultipartFormDataContent())
                    {
                        // Añadir parámetros
                        var correlationIdContent = new StringContent(Guid.NewGuid().ToString());
                        var activityCorrelationIdContent = new StringContent(activityCorrelationId);
                        var channelCorrelationIdContent = new StringContent(channelCorrelationId);

                        multiFormContent.Add(correlationIdContent, "\"correlationId\"");
                        multiFormContent.Add(activityCorrelationIdContent, "\"activityCorrelationId\"");
                        multiFormContent.Add(channelCorrelationIdContent, "\"channelCorrelationId\"");

                        var fileData = stream.ToArray();
                        var fileContent = new ByteArrayContent(fileData, 0, fileData.Length);

                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "\"file\"",
                            FileName = $"\"integration_file.zip\"",
                        };
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-zip-compressed");
                        multiFormContent.Add(fileContent);

                        var response = await httpClient.PostAsync("api/integrationFiles/", multiFormContent);

                    }

                }

            }

        }

    }
}
