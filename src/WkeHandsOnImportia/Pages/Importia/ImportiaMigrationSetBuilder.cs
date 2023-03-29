using a3innuva.TAA.Migration.SDK.Implementations;
using a3innuva.TAA.Migration.SDK.Interfaces;

namespace WkeHandsOnImportia.Pages.Importia
{
    public static class ImportiaMigrationSetBuilder
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
