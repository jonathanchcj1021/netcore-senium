using System;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;
//using Google.Apis.Vision.v1.Data;

namespace NetCoreSelenium.Library
{
    public class DetectorHelper
    {
        public static LogHelper log = new LogHelper(typeof(DetectorHelper));
        public DetectorHelper()
        {
        }
       
        public static int DetectFromImage(string fileName, out string result )
        {
            bool isSuccess = false;
            result = "";
            try
            {
              
                var client = ImageAnnotatorClient.Create();
                //var image = Image.FromUri("gs://cloud-samples-data/vision/using_curl/shanghai.jpeg"); 

                var image = Image.FromFile(fileName);
                //var labels = client.DetectLabels(image2); 
                //var t = client.DetectImageProperties(image2);
                var response = client.DetectText(image);

                foreach (var annotation in response)
                {
                    result = annotation.Description;
                }


                log.Info($"Detect image -> {result}");
                log.Info($"Detect isSuccess -> {isSuccess}");

                return result.Length;
    
             
            }
            catch(Exception)
            {
                throw;
            }
            
           
        }
    }
}
