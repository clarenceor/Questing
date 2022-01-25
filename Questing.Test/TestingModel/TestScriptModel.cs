using Newtonsoft.Json.Linq;
using Questing.Data.Model.Request;
using Questing.Data.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Test.TestingModel
{
    public class TestScriptModel
    {
        public int RequestSequence { get; init; }
        public string RequestUrl { get; init; }
        public string RequestType { get; init; }
        public string RequestJson { get; set; }
        public string ExpectedResponseJson { get; set; }
        public bool PostRequest
        {
            get
            {
                return RequestType == "POST";
            }
        }

        public Type GetResponseClassType()
        {
            if (RequestUrl.StartsWith("api/state"))
                return typeof(GetUserQuestStateRes);
            else
                return typeof(GetUserQuestProgressRes);
        }

        public Type GetRequestClassType()
        {
            if (!RequestUrl.StartsWith("api/state"))
                return typeof(GetUserQuestProgressReq);
            else
                return default;
        }

        public void ProcessNode()
        {

        }
    }
}
