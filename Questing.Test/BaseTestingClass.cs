using Newtonsoft.Json;
using Questing.Test.TestingModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questing.Test
{
    public class BaseTestingClass
    {
        protected string TemplateBasePath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        protected List<TestScriptModel> testScriptModels = new List<TestScriptModel>();
        protected Dictionary<int, string> FailCase = new Dictionary<int, string>();

        public virtual void LoadTestScriptFromDirectory(string SubDirectoryWithFileName)
        {
            string TemplatePath = Path.Combine(TemplateBasePath, SubDirectoryWithFileName);
            string TemplateString = File.ReadAllText(TemplatePath);

            if (!string.IsNullOrEmpty(TemplateString))
                testScriptModels = JsonConvert.DeserializeObject<List<TestScriptModel>>(TemplateString);
            else
                testScriptModels = new List<TestScriptModel>();
        }

        public virtual void PrintResult()
        {
            if (FailCase.Count > 0)
            {
                StringBuilder strBuilder = new StringBuilder();

                foreach (var fail in FailCase)
                    strBuilder.AppendLine(fail.Value);

                throw new Exception(strBuilder.ToString());
            }
        }
    }
}
