using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace WebApplication3.general
{
    public static class Upload 

    {
     
        public static string upload_image(string path,HttpPostedFileBase upload)
        {
        
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (upload != null)
            {
                DateTime time = DateTime.Now;
                string format = "HHmmss";
                string date = time.ToString(format);
                string name = Path.GetFileNameWithoutExtension(upload.FileName);
                var extension = Path.GetExtension(upload.FileName);
                string FullName = name + "_" + date + "_" + extension;
                upload.SaveAs(Path.Combine(path, FullName));
                return FullName;

            }
            return null;
        }
        public static void delete_Image(string path,string ImageName)
        {
            if (ImageName!=null)
            {
                string p = Path.Combine(path, ImageName);
                System.IO.File.Delete(p);
            }
           
        }
    }
}