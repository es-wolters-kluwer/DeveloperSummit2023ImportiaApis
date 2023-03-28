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

    public async Task OnPostAsync()
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

    var companyCorrelationId = $"1a73b3c0-89c5-4385-9df7-6c3fdc6f9bd7";
    var activityCorrelationId = $"96bee4a4-1a17-43d6-9a47-91f000f3419b";
    var channelCorrelationId = $"3428a766-8132-48f2-a535-9f9c78e04951";

``` 

You can get this information using the a3innuva | contabilidad APIs

## Try the application and upload a file to a3innuva directly from out application.