# Generate an importia Migration Set

## Añadir los paquete de a3innuva.importia.sdk

    a3innuva.Importia.SDK.Interfaces
    a3innuva.Importia.SDK.Implementations
    a3innuva.Importia.SDK.Serialization
    a3innuva.Importia.SDK.Extensions

## Create the ImportiaMigrationSetBuilder.cs inside the folder Importia

This class is responsable of creating the importia migration set

```

    using a3innuva.TAA.Migration.SDK.Implementations;
    using a3innuva.TAA.Migration.SDK.Interfaces;

    namespace WkeHandsOnImportia.Pages.Importia
    {

        public class ImportiaMigrationSetBuilder
        {

            public static IMigrationSet CreateMigrationSet() =>
                new MigrationSet()
                {
                    Info = new MigrationInfo()
                    {
                        VatNumber = "B02731792",
                        Year = 2023,
                        Type = MigrationType.OutputInvoice,
                        Origin = MigrationOrigin.Extern,
                        FileName = $"output_invoices_{Guid.NewGuid()}.csv",
                        Version = "2.0"
                    },
                    Entities = CreateMigrationEntities()
                };

            private static IMigrationEntity[] CreateMigrationEntities()
            {

                var migrationSet = new List<IMigrationEntity>();

                foreach (var index in Enumerable.Range(1, 10))
                {

                    var invoice = new OutputInvoice()
                    {
                        Id = Guid.NewGuid(),
                        Line = 1,
                        Source = "extern",
                        InvoiceDate = DateTime.Now,
                        JournalDate = DateTime.Now,
                        TransactionDate = DateTime.Now,
                        InvoiceNumber = $"SAC_{index.ToString("000")}",
                        Lines = new IOutputInvoiceLine[]
                        {
                            new OutputInvoiceLine()
                            {
                                Id = Guid.NewGuid(),
                                Line = 1,
                                Index = 0,
                                BaseAmount = 1000,
                                CounterPart = "7050000000",
                                CounterPartDescription = "PRESTACIONES DE SERVICIOS",
                                TaxAmount = 210,
                                TaxSurchargeAmount = 0,
                                TaxDeductibleAmount = 0,
                                TaxNonDeductibleAmount = 0,
                                TaxDeductibleSurchargeAmount = 0,
                                TaxNonDeductibleSurchargeAmount = 0,
                                WithHoldingPercentage = 0,
                                WithHoldingAmount = 0,
                                Transaction = "OP_INT",
                                WithHolding = null,
                                TaxCode = ""
                            }
                        },
                        PartnerName = "Ferretería amigos, s.l.",
                        VatNumber = "46776910Q",
                        PartnerAccount = "4300000001",
                        IsCorrective = false,
                        CorrectiveInvoiceNumber = "",
                        PostalCode = "08788",
                        CountryCode = "ES",
                        VatType = 2,
                        AccountingAffected = true,
                        PendingAmount = 0,
                        SatisfiedAmount = 0,
                        Maturities = new ICharge[] { }
                    };

                    migrationSet.Add(invoice);

                }

                return migrationSet.ToArray();

            }

        }

    }

```


## Create the ImportiaZipBuilder.cs class inside the folder Importia

this class is responsable of zip a migration set

```

    using a3innuva.TAA.Migration.SDK.Interfaces;
    using Newtonsoft.Json;
    using System.IO.Compression;
    using System.Text;

    namespace WkeHandsOnImportia.Pages.Importia
    {
        public class ImportiaZipBuilder
        {

            public byte[] CreateMigrationSetZipFile(IMigrationSet migrationSet)
            {

                // the output bytes of the zip
                byte[]? fileBytes = null;

                // create a working memory stream
                using (var memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {

                        // add the item name to the zip
                        ZipArchiveEntry zipItem = zip.CreateEntry(migrationSet.Info.FileName);
                        MemoryStream migrationSetStream = this.ConvertMigrationSetToStream(migrationSet);

                        // add the item bytes to the zip entry by opening the original file and copying the bytes
                        using (Stream entryStream = zipItem.Open())
                        {
                            migrationSetStream.CopyTo(entryStream);
                        }

                    }

                    fileBytes = memoryStream.ToArray();

                    return fileBytes;
                }

            }

            private MemoryStream ConvertMigrationSetToStream(IMigrationSet migrationSet)
            {

                var ms = new MemoryStream();

                using (var memoryStream = new MemoryStream())
                {

                    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    {

                        using JsonTextWriter writer = new JsonTextWriter(streamWriter);
                        {

                            var serializationSettings = this.GetSerializationSettings();
                            string json = JsonConvert.SerializeObject(migrationSet, serializationSettings);
                            writer.WriteRaw(json);
                            writer.Flush();

                            memoryStream.Seek(0, SeekOrigin.Begin);
                            memoryStream.CopyTo(ms);

                            ms.Seek(0, SeekOrigin.Begin);

                        }

                    }

                }

                return ms;

            }

            private JsonSerializerSettings GetSerializationSettings(Formatting type = Formatting.None)
            {
                return new JsonSerializerSettings
                {
                    Formatting = type,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                    TypeNameHandling = TypeNameHandling.All,
                    DateParseHandling = DateParseHandling.DateTime,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
            }

        }
    }

```


## Add an Index razor page 
	
 ![img](images/2.4.-%20Add%20the%20importia%20razor%20page.PNG)

## Inside de Index.cshtml, let's add the html form code to send the request to generate an importia migration set
	
```

    @page
    @model WkeHandsOnImportia.Pages.Importia.IndexModel
    @{
    }

    <div class="row">
        <div class="col-6">
            <form asp-page-handler="GenerateIntegrationFile" method="post">
                <button type="submit" class="btn btn-primary btn-lg">
                    Generate integration file
                </button>
            </form>
        </div>
    </div>

```

		
## Add backend handler for the GenerateIntegrationFile form request

```

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    namespace WkeHandsOnImportia.Pages.Importia
    {

        [Authorize]
        public class IndexModel : PageModel
        {

            public void OnGet()
            {
            }

            // Generar un fichero de importia
            public IActionResult OnPostGenerateIntegrationFile()
            {

                var migrationSet = ImportiaMigrationSetBuilder.CreateMigrationSet();
                var migrationSetZipFile = new ImportiaZipBuilder().CreateMigrationSetZipFile(migrationSet);

                return File(migrationSetZipFile, "application/octet-stream", $"{migrationSet.Info.FileName}.zip");

            }

        }
    }

```


	
## Add and html link button in the /Pages/Index.cshtml page to navigate to the /Importia/Index.cshtml Page
	
```

    <a asp-area="" asp-page="/Importia/Index" class="btn btn-primary btn-lg">
        Importia
    </a>

```

## Start the application and try to generate and importia migration set, save it into the local computer and upload it to a3innuva manually.