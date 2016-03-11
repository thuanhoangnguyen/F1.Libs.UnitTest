using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1.Libs.UnitTest
{    
    internal class DefaultReport:ITestReport
    {
        internal string Output { get; set; }
        public DefaultReport(string outputPath)
        {
            Output = outputPath;
        }

        public void Render(List<TestData> TestCases)
        {
            var output = string.Empty;
            foreach (var testCase in TestCases)
            {
                output += "Execute " + testCase.Desc+"\r\n";
                for (var i = 0; i < testCase.TestItems.Count; i++)
                {
                    var t = testCase.TestItems[i];
                    output += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", i, t.Input, t.EOP, t.AOP, t.Result);
                }
            }

            using (var tw = new StreamWriter(Output, true))
            {
                tw.WriteLine(output);
                tw.Close();
            }            
        }
    }
}
