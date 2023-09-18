using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.AzureBlobService.Blob.Dtos
{
    public class BlobDetails
    {
        public string? BlobConnectionString { get; set; }
        public string? BlobContainerVideos { get; set; }
        public string? BlobContainerImages { get; set; }
    }
}
