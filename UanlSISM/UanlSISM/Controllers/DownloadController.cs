using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public ActionResult APK()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/Imagenes/smuanl.apk");
            string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
            var MyFile = new FileContentResult(fileBytes, contentType);
            MyFile.FileDownloadName = "smuanl.apk";
            return MyFile;
        }

      
    }
}
