﻿using AutoMapper;
using BusinessLayer.BusinessObject;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Sportclub.App_Start;
using Sportclub.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sportclub.Controllers
{
    public class BlobHelper
    {
        const string blobContainerName = "containerblob";
        private const string uripath = "https://storageblobitstep.blob.core.windows.net/containerblob";

        public static async Task<ImageBO> SetImageAsync(HttpPostedFileBase upload, ImageVM imageVM, ImageBO imageBase, UserBO userBO, IMapper mapper)
        {
            //а)------запись в blobStorage ----------------------            
            string filename = Path.GetFileName(upload.FileName);
            bool resUpload = await UploadFile(upload);
            //б)-----запись в БД----------
            if (resUpload == true) {
                string uriStr = uripath + "/" + filename;
                imageVM.Filename = filename;
                imageVM.URI = new Uri(uriStr);
                var imgListBO = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
                if (imgListBO == null || imgListBO.Count() == 0)                    //если такого в БД нет - сохранить
                {
                    var imageBO = mapper.Map<ImageBO>(imageVM);
                    imageBase.Save(imageBO);
                }
                List<ImageBO> imageBases = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
                imageBase = imageBases[0];
                userBO.ImageId = imageBase.Id;
            }
            return imageBase;
        }

        public static async Task<bool> UploadFile(HttpPostedFileBase upload)
        {
            try {
                string filename = Path.GetFileName(upload.FileName);
                string storagekey = ConfigurationManager.ConnectionStrings["blobContainer"].ConnectionString;  
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storagekey);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(blobContainerName); //.. .windows.net/containerblob";
                container.SetPermissions(  new BlobContainerPermissions {  PublicAccess = BlobContainerPublicAccessType.Blob });
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                using (var fileStream = upload.InputStream) {  //System.IO.File.OpenRead(@"path\myfile")
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
                return true;
            }
            catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("Upload Error: " + ex.Message);
                return false;
            }
        }

        public static List<string> DowloadUriBackground()
        {
            string storageKey = ConfigurationManager.ConnectionStrings["blobBoxBackground"].ConnectionString;
            CloudStorageAccount cloud = CloudStorageAccount.Parse(storageKey);
            CloudBlobClient blobClient = cloud.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("backgrounds");
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            List<string> uriList = new List<string>();
            
            var res =  container.ListBlobs().Where(b=> b.GetType()==typeof(CloudBlockBlob)).Select(b =>b.Uri.ToString());
            return res.ToList();
        }
    }
}