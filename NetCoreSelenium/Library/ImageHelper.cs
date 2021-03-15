using System;
using System.IO;

namespace NetCoreSelenium.Library
{
    public class ImageHelper
    {
        public ImageHelper()
        {
             
        }
        public static void ImageFromBase64(string base64Img, string fileName)
        {
            var bytes = Convert.FromBase64String(base64Img);
            using (var img = new FileStream(fileName, FileMode.Create))
            {
                img.Write(bytes, 0, bytes.Length);
                img.Flush();
            }
        }
    }
}
