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
    class TestCaseRunTask : TCAddOnTask
    {

        static HttpClient client = new HttpClient();

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            taskContext.ShowMessageBox("Attention", "This entry will be run via an AddOnTask");
            RunAsync(objectToExecuteOn).Wait();
            return null;
        }

        public override string Name => "PushNow";

        public override Type ApplicableType => typeof(TestCase);

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

            

            //assign BaseUrl into http client
            client.BaseAddress = new Uri(ZUtil.BASE_URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var RELATIVE_PATH = "flex/services/rest/latest/testcase";
            var QUERY_STRING = "";

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ZUtil.USER + ":" + ZUtil.PASSWORD));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
            //client.DefaultRequestHeaders.Add("zapiAccessKey", ACCESS_KEY);
            //client.DefaultRequestHeaders.Add("User-Agent", "ZAPI");
              

            TestCase testCases = (TestCase)objectToExecuteOn;
            List<Object> resultTestCase = new List<Object>();
            string testCaseName = testCases.DisplayedName;

            if (testCases.Items != null && testCases.Items.Count() > 0) {
                List<Object> ts = new List<Object>();
                foreach (object obj in testCases.Items)
                {
                    TestStep testStep = (TestStep)obj;
                    var tsObj = new { name = testStep.DisplayedName, uniqueId = testStep.UniqueId };
                    ts.Add(tsObj);
                }
                var testCase = new
                {
                    name = testCases.DisplayedName,
                    uniqueId = testCases.UniqueId,
                    testSteps = ts.ToArray(),
                    tree = new { name = testCases.NodePath }
                };
                resultTestCase.Add(testCase);
            }
 
            try
            {
                HttpResponseMessage response = await client.PostAsync(ZUtil.CONTEXT_PATH + RELATIVE_PATH + "?" + QUERY_STRING,
                    new StringContent(JsonConvert.SerializeObject(resultTestCase.ToArray()).ToString(),
                            Encoding.UTF8, ZUtil.CONTENT_TYPE_JSON));
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
