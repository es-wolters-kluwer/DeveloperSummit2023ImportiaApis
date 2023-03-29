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
