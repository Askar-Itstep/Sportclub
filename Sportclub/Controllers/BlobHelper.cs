using AutoMapper;
using BusinessLayer.BusinessObject;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Sportclub.App_Start;
using Sportclub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sportclub.Controllers
{
    public class BlobHelper
    {
        const string blobContainerName = "containerblob";
        private const string storagekey = "DefaultEndpointsProtocol=https;AccountName=storageblobitstep;AccountKey=iS0bPloKlG1EBzuUsjlRf7PYXFAUHZu+omLC5FjVFnnc/yn64Zp6MYqlg+7Wu15Vx32OGQK8nhr22AEE4rfhjQ==;EndpointSuffix=core.windows.net";
        private const string uripath = "https://storageblobitstep.blob.core.windows.net/containerblob";
        static CloudBlobContainer blobContainer = new CloudBlobContainer(new Uri(uripath));

        public static async Task<ImageBO> SetImageAsync(HttpPostedFileBase upload, ImageVM imageVM, ImageBO imageBase, UserBO userBO, IMapper mapper)
        {
            string filename = System.IO.Path.GetFileName(upload.FileName);
            imageVM.Filename = filename;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storagekey);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient(); // Create the blob client.            
            CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName);// Retrieve reference to a previously created container.            
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);// Retrieve reference to a blob named "myblob".
                                         
            using (var fileStream = upload.InputStream) {  //System.IO.File.OpenRead(@"path\myfile")
                 await blockBlob.UploadFromStreamAsync(fileStream);
            }
            string uriStr = uripath + "/" + filename;
            imageVM.URI = new Uri(uriStr);
            //запись в БД
            var imgListBO = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            if (imgListBO == null || imgListBO.Count() == 0)                    //если такого в БД нет - сохранить
            {
                var imageBO = mapper.Map<ImageBO>(imageVM);
                imageBase.Save(imageBO);
            }
            List<ImageBO> imageBases = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            imageBase = imageBases[0];
            userBO.ImageId = imageBase.Id;
            return imageBase;
        }
    }
}