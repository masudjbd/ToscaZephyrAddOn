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
    public class ExecutionRunTask : TCAddOnTask
    {
        static HttpClient client = new HttpClient();

        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {

            taskContext.ShowMessageBox("Attention", "Executed selected Item");
            RunAsync(objectToExecuteOn).Wait();
            return null;

        }



        public override string Name => "Execute";

        public override Type ApplicableType => typeof(ExecutionListItem);

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

            var RELATIVE_PATH = "flex/services/rest/latest/execution/create";
            var QUERY_STRING = "";

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(ZUtil.USER + ":" + ZUtil.PASSWORD));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

            ExecutionListItem executionListItem = (ExecutionListItem)objectToExecuteOn;
            ExecutionList executionEntryList = executionListItem.ExecutionList;
            ExecutionEntryFolder executionEntryFolder = (ExecutionEntryFolder)executionEntryList.Items.First();
            List<Object> testCase = new List<Object>();

            string execStatus = "-1";
            for (int i = 0; i < executionEntryFolder.Items.Count(); i++)
            {
                ExecutionEntry execItem = (ExecutionEntry)executionEntryFolder.Items.ElementAt(i);
                ExecutionResult execResult = execItem.ActualResult;
                if (execResult.Equals("Passed"))
                { execStatus = "1"; }
                else { execStatus = "2"; }
                List<Object> testSteps = new List<Object>();
                TestCase testCaseItem = (TestCase)execItem.TestCase;
                for (int j = 0; j < testCaseItem.Items.Count(); j++)
                {
                    TestStep ts = (TestStep)testCaseItem.Items.ElementAt(j);
                    var tsObj = new { name = ts.DisplayedName };
                    testSteps.Add(tsObj);
                }
                var tc = new
                {
                    name = execItem.DisplayedName,
                    executionResult = execStatus,
                    testSteps = testSteps.ToArray()
                };
                testCase.Add(tc);
            }
            var jsonContent = new
            {
                testCases = testCase.ToArray(),
                executionName = executionListItem.DisplayedName,
                executionResult = execStatus,
                releaseId = "1",
                folderName = executionListItem.DisplayedName
            };


            try
            {
                HttpResponseMessage response = await client.PostAsync(ZUtil.CONTEXT_PATH + RELATIVE_PATH + "?" + QUERY_STRING,
                    new StringContent(JsonConvert.SerializeObject(jsonContent).ToString(),
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
