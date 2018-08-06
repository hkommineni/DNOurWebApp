
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.IO;
using System.Net;

namespace CSharpARMDeployer
{
    class Program
    {
        public static string tenantId = "4d765082-a8b4-4546-91d5-764b5f1fe41e";
        public static string applicationId = "67413dc8-6d5e-4174-a45c-5530d0f487de";
        public static string clientSecret = "oeQTN4YivUSPllGtj1gc0WyJcTdpmJ5PfzE7IlkkFKY=";

        static void Main(string[] args)
        {
            DeployTemplate(GetToken());
        }

        public static string GetToken()
        {
            var url = "https://login.windows.net/" + tenantId;

            var authenticationContext = new AuthenticationContext(url);
            var credential = new ClientCredential(applicationId, clientSecret);

            return authenticationContext.AcquireTokenAsync("https://management.azure.com/", credential).Result.AccessToken;
        }

        private static void DeployTemplate(string accessToken)
        {
            var url = "https://management.azure.com/subscriptions/d21b2487-e35d-4d9c-ba9c-855501096b32/resourcegroups/CSharpARMTemplate/providers/Microsoft.Resources/deployments/CSharpDeployment?api-version=2015-01-01";

            var request = (HttpWebRequest)WebRequest.Create(url);

            var putBody = @"{
             ""properties"": {
               ""templateLink"": {
                            ""uri"": ""https://cs7d21b2487e35dx4d9cxba9.blob.core.windows.net/templates/WebSiteSQLDatabase.json"",
                 ""contentVersion"": ""1.0.0.0""
               },
               ""mode"": ""Incremental"",
               ""parametersLink"": {
                            ""uri"": ""https://cs7d21b2487e35dx4d9cxba9.blob.core.windows.net/templates/WebSiteSQLDatabase.parameters.json"",
                 ""contentVersion"": ""1.0.0.0""
               }
                    }
                }";

            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            request.ContentType = "application/json";
            request.Method = "PUT";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(putBody);
                writer.Flush();
                writer.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    throw new InvalidOperationException("Request failed with the following result: " + result + '\n' + response.ToString());
                }
            }
        }
    }
}
