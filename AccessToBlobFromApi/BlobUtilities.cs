using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace OktaApi
{
    public class BlobUtilities
    {
        public static CloudBlobClient BlobClient
            => CloudStorageAccount.Parse(Constants.StorageConnectionString).CreateCloudBlobClient();

        public static async Task<byte[]> DownloadFileFromBlob(string FileName)
        {
            var container = BlobClient.GetContainerReference(Constants.DocumentsContainer);
            var blockBlob = container.GetBlockBlobReference(FileName);

            if(! await blockBlob.ExistsAsync())
            {
                return null;
            }

            // Read content
            using (var ms = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
