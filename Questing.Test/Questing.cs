using Newtonsoft.Json;
using Questing.Controllers;
using Questing.Data.Model.Request;
using Questing.Data.Model.Response;
using Questing.IServices;
using Questing.Services;
using Questing.Test.TestingModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Questing.Test
{
    public class Questing : BaseTestingClass
    {
        private string TestingEndPoint = "https://localhost:44342/";

        [Fact]
        public async Task StartTesting()
        {
            LoadTestScriptFromDirectory("TestingTemplate\\Questing\\QuestTestingTemplate.json");

            foreach (var model in testScriptModels)
            {
                Type responseType = model.GetResponseClassType();

                if (model.PostRequest)
                {
                    if (responseType == typeof(GetUserQuestProgressRes))
                    {
                        try
                        {
                            var ExpectedResponse = JsonConvert.DeserializeObject<BasicResponse<GetUserQuestProgressRes>>(model.ExpectedResponseJson);
                            var ActualResponse = await APICallWithJson<BasicResponse<GetUserQuestProgressRes>>(model.RequestUrl, model.RequestJson, model.PostRequest);

                            if (ExpectedResponse != null)
                            {
                                bool MatchedResult = ExpectedResponse.Content.Equals(ActualResponse.Content);

                                if (!MatchedResult)
                                    FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Result not matched.");
                            }
                            else
                            {
                                bool MatchedResult = ExpectedResponse == null && ActualResponse == null;

                                if (!MatchedResult)
                                    FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Result not matched.");
                            }
                        }
                        catch (Exception ex)
                        {
                            FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Exceptions: " + ex.ToString());
                        }
                    }
                }
                else
                {
                    if (responseType == typeof(GetUserQuestStateRes))
                    {
                        try
                        {
                            var ExpectedResponse = JsonConvert.DeserializeObject<BasicResponse<GetUserQuestStateRes>>(model.ExpectedResponseJson);
                            var ActualResponse = await APICallWithJson<BasicResponse<GetUserQuestStateRes>>(model.RequestUrl, model.RequestJson, model.PostRequest);

                            if (ExpectedResponse != null)
                            {
                                bool MatchedResult = ExpectedResponse.Content.Equals(ActualResponse.Content);

                                if (!MatchedResult)
                                    FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Result not matched.");
                            }
                            else
                            {
                                bool MatchedResult = ExpectedResponse == null && ActualResponse == null;

                                if (!MatchedResult)
                                    FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Result not matched.");
                            }
                        }
                        catch (Exception ex)
                        {
                            FailCase.Add(model.RequestSequence, $"Request Sequence {model.RequestSequence} Exceptions: " + ex.ToString());
                        }
                    }
                }
            }

            PrintResult();
        }

        private async Task<T> APICallWithJson<T>(string URL, string requestJson, bool PostRequest)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(TestingEndPoint);
                HttpResponseMessage httpResponse = null;

                if (PostRequest)
                {
                    var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    httpResponse = await httpClient.PostAsync(URL, httpContent);
                }
                else
                {
                    httpResponse = await httpClient.GetAsync(URL);
                }

                using (var response = httpResponse)
                {
                    string ActualResponseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(ActualResponseJson.ToString());
                }
            }
        }
    }
}