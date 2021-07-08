using Newtonsoft.Json;
using SuperLocker.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace SuperLocker.Functional.Tests.Steps
{
    [Binding]
    public class Commands_UnlockSteps
    {
        private readonly UnlockRequest _unlockRequest;
        private  Response response;
        public Commands_UnlockSteps()
        {
            _unlockRequest = new UnlockRequest();
            response = new Response();
        }

        [Given(@"the userId is (.*)")]
        public void GivenTheUserIdIs(string userId)
        {
            _unlockRequest.UserId = Guid.Parse(userId);
        }

        [Given(@"the lockId is (.*)")]
        public void GivenTheLockIdIsAb_Cf_Addbfbf(string lockId)
        {
            _unlockRequest.UserId = Guid.Parse(lockId);
        }

        [When(@"requted for unlock")]
        public async Task WhenRequtedForUnlock()
        {
            using var client = new HttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("userId", _unlockRequest.UserId.ToString()),
                new KeyValuePair<string, string>("lockId", _unlockRequest.LockId.ToString())
            });

            var result = await client.PostAsync("http://localhost:5000/lock/unlock",
                new StringContent(JsonConvert.SerializeObject(_unlockRequest)
                , Encoding.UTF8,"application/json"));

    
            var responseString = await result.Content.ReadAsStringAsync();
             response = JsonConvert.DeserializeObject<Response>(responseString);

        }

        [Then(@"the result should have status (.*)")]
        public void ThenTheResultShouldHaveStatus(int statusCode)
        {
            Assert.Equal(statusCode, response.status);
        }

        [Then(@"the error should contain (.*)")]
        public void ThenTheResultShouldHaveStatus(string lockId)
        {
            Assert.Contains(lockId, response.errors.Keys);
        }
    }


    public class Response
    {
        public int status { get; set; }
        public Dictionary<string,List<string>> errors { get; set; }
    }

}
