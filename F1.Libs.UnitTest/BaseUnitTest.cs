using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1.Libs.UnitTest
{
    public class BaseUnitTest:IUnitTest
    {
        protected virtual string DataPath
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual string OutputPath
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual string Name
        {
            get { throw new NotImplementedException(); }
        }

        protected List<TestData> testCases = null;

        protected ITestReport Report;

        protected virtual void ReadData()
        {
            using (var tr = new StreamReader(DataPath))
            {
                testCases = JsonConvert.DeserializeObject<List<TestData>>(tr.ReadToEnd());
                tr.Close();
            }
        }

        protected virtual void Execute(TestData testCase)
        {
            testCase.Compare();
        }        

        public void Execute()
        {
            ReadData();
            ITestReport testReport = Report;
            if (testReport == null)
            {
                testReport = new DefaultReport(OutputPath);
            }

            foreach (var testCase in testCases)
            {
                Execute(testCase);
            }

            testReport.Render(testCases);
        }
    }
}
