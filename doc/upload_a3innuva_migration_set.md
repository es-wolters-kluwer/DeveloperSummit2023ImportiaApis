# Upload the generated Importia Migration Set to a3innuva

## Update the page /Importia/Index.cshtml and add a form to upload a file.

```

    <div class="row">
        <div class="col-6">
            <form asp-page-handler="GenerateIntegrationFile" method="post">
                <button type="submit" class="btn btn-primary btn-lg">
                    Generate integration file
                </button>
            </form>
        </div>
        <div class="col-6">
            <form asp-page-handler="UploadFile" method="post" enctype="multipart/form-data">
                <input type="file" asp-for="IntegrationFile" />
                <button type="submit" class="btn btn-primary btn-lg">
                    Upload integration file
                </button>
            </form>
        </div>
    </div>

```

## Add the backend handler to upload the file to a3innuva using the Importia API.

```

    [BindProperty]
    public IFormFile IntegrationFile { get; set; }

    public async Task OnPostUploadFile()
    {

        using (var stream = new MemoryStream())
        {
            await IntegrationFile.CopyToAsync(stream);

            var baseUrl = $"https://a3api.wolterskluwer.es/a3innuva-contabilidad-importia/";

            var companyCorrelationId = $"";
            var activityCorrelationId = $"";
            var channelCorrelationId = $"";

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

```

### IMPORTANT: 

For this sample, a pre-configured company will be used. 

```

    var companyCorrelationId = $"27200f28-b59c-4f63-aa89-7fabcf16f96f";
    var activityCorrelationId = $"62fd87e8-4050-4bf3-bab1-8c02f1e9f716";
    var channelCorrelationId = $"819cc23f-d62b-4336-b98c-37fce9fbcd75";

``` 

You can get this information using the a3innuva | contabilidad APIs

## Try the application and upload a file to a3innuva directly from out application.