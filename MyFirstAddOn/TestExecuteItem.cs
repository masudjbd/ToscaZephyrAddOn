using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Newtonsoft.Json;

namespace ZephyrAddOn
{
    class TestExecuteItem : TCAddOnMenuItem
    {
       
        static HttpClient client = new HttpClient();

        public override void Execute(TCAddOnTaskContext context)
        {

            //context.ShowMessageBox("Test Execute", "You clicked on a menu item.");
            //RunAsync().Wait();
            //Console.ReadLine();
            context.ShowMessageBox("Execute Result", "Successfully Executed on ZEE");
        }
        public override string ID => "TestExecute";
        public override string MenuText => "Test Execute";

        public override string MenuItemImageName => "zephyr.ico";




        static async Task RunAsync()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
           delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                   System.Security.Cryptography.X509Certificates.X509Chain chain,
                   System.Net.Security.SslPolicyErrors sslPolicyErrors)
           {
               return true;
           };

           

           
            //assign BaseUrl into http client
            client.BaseAddress = new Uri(ZUtil.BASE_URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var RELATIVE_PATH = "flex/services/rest/v1/execution/6962";
            var QUERY_STRING = "";

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ZUtil.USER + ":" + ZUtil.PASSWORD));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
            //client.DefaultRequestHeaders.Add("zapiAccessKey", ACCESS_KEY);
            //client.DefaultRequestHeaders.Add("User-Agent", "ZAPI");
            var paramContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("param1Value", "param1"),
                new KeyValuePair<string, string>("param2Value", "param2"),
                new KeyValuePair<string, string>("param3Value", "param3")
            });

            var jsonContent = new
            {
                lastTestResult = new 
                    {
                        executionStatus = 2
                    }
        };

            var issueContent = new
            {
                //{"fields":{"project":{"key":"SAM"},"summary":"Summary sample.","description":"Desc sample.","issuetype":{"name":"Test"}}}
                fields = new {
                    project = new { key = "SAM" },
                    summary = "Summary from Tosca",
                    description = "Description from Tosca",
                    issuetype = new { name = "Test"}
                }
            };

            
             //  new StringContent(JsonConvert.SerializeObject(issueContent).ToString(),
             //               Encoding.UTF8, "application/json")
              
             

            try
            {
                HttpResponseMessage response = await client.PutAsync(ZUtil.CONTEXT_PATH + RELATIVE_PATH + "?" + QUERY_STRING,
                    new StringContent(JsonConvert.SerializeObject(jsonContent).ToString(),
                            Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                //write response in console
                Console.WriteLine(response);

                // Deserialize the updated product from the response body.
                string result = await response.Content.ReadAsStringAsync();

                //write Response in console
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

 

    }
}
