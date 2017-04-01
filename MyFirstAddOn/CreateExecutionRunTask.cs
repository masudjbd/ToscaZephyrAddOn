using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace ZephyrAddOn
{
    public class CreateExecutionRunTask : TCAddOnTask
    {
        static HttpClient client = new HttpClient();

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {

            taskContext.ShowMessageBox("Attention", "This entry will be run via an AddOnTask");
            RunAsync(objectToExecuteOn).Wait();
            return null;

        }

        public override string Name => "Create Execute";

        public override Type ApplicableType => typeof(ExecutionList);

        public override bool IsTaskPossible(TCObject obj) { return true; }

        public override bool RequiresChangeRights => true;



        static async Task RunAsync(TCObject objectToExecuteOn)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
           delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                   System.Security.Cryptography.X509Certificates.X509Chain chain,
                   System.Net.Security.SslPolicyErrors sslPolicyErrors)
           {
               return true;
           };

            //define userName
            var USER = "test.manager";

            //define passWord
            var PASSWORD = "test.manager";

            //define Base Url
            var BASE_URL = "http://tricentis.yourzephyr.com";

            //define ContextPath
            var CONTEXT_PATH = "/";


            //assign BaseUrl into http client
            client.BaseAddress = new Uri(BASE_URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var RELATIVE_PATH = "flex/services/rest/latest/execution/create";
            var QUERY_STRING = "";

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(USER + ":" + PASSWORD));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
            //client.DefaultRequestHeaders.Add("zapiAccessKey", ACCESS_KEY);
            //client.DefaultRequestHeaders.Add("User-Agent", "ZAPI");

            var jsonContent = new
                {
                    testcaseName = "ToscaZEEAutomation",
                    testSteps = new[] {
                                    "Open Browser",
                                    "Enter Login Credential"
                                    },
                    executionName = "Cycle 1",
                    executionResult = true
                };

            jsonContent.testcaseName = "some Value";


            try
            {
                HttpResponseMessage response = await client.PutAsync(CONTEXT_PATH + RELATIVE_PATH + "?" + QUERY_STRING,
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
