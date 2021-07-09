using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace SuperLocker.Functional.Tests.Steps
{
    [Binding]
    public class ApplicationUsersRequireATokenToMakeRequestsSteps
    {

        private Table givenCredentials;
        private Dictionary<string, bool> resultTable;

        public ApplicationUsersRequireATokenToMakeRequestsSteps()
        {
            resultTable = new Dictionary<string, bool>();
        }
        

        [Given(@"following 2 users having")]
        public void GivenFollowingUsersHaving(Table table)
        {
            this.givenCredentials = table;
        }
        
        [When(@"they request for token")]
        public async Task WhenTheyRequestForToken()
        {
            foreach (var row in givenCredentials.Rows)
            {
                var userName = row[0];
                var password = row[1];

                resultTable[userName] = await CheckIfGotToken(userName, password);
            }
        }



        [Then(@"the token result should be")]
        public void ThenTheTokenResutlShouldBe(Table table)
        {
            foreach (var row in table.Rows)
            {
                var userName = row[0];
                var isValid = row[1];
                Assert.True(resultTable[userName].ToString().Equals(isValid, System.StringComparison.InvariantCultureIgnoreCase));
            } 
        }


        private static async Task<bool> CheckIfGotToken(string userName, string password)
        {
            var client = new RestClient("https://localhost:5003/connect/token")
            {
                Timeout = -1
            };

            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", "client");
            request.AddParameter("client_secret", "secret");
            request.AddParameter("scope", "super_lock_api");
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", userName);
            request.AddParameter("password", password);
            var response = await client.ExecuteAsync(request);
           

            return response.IsSuccessful && response.Content.Contains("access_token");
        }
    }
}
