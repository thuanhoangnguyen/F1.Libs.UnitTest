using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1.Libs.UnitTest.Console
{
    public class MyUnitTest : BaseUnitTest
    {
        protected override string DataPath
        {
            get { return "MyUnitTest.dat"; }
        }

        protected override string OutputPath
        {
            get { return "MyUnitTest.out"; }
        }

        protected override string Name
        {
            get { return "MyUnitTest"; }
        }        

        private bool _compareData(TestDataItem item)
        {
            if (item.AOP != item.EOP)
                return false;
            return true;
        }

        protected override void Execute(TestData testCase)
        {
            // implement comparision and call url here
            // here, I implement a simple processing call:
            // item.AOP (ActualOutput) will get by REST/POST and transform or you can implement your own comparision using CompareAction

            foreach (var item in testCase.TestItems)
            {
                if (item.Input.Contains("b"))
                    item.AOP = item.EOP;
                else
                    item.AOP = "haha";
            }

            testCase.CompareAction = _compareData;
            base.Execute(testCase);
        }
    };

    class Program
    {
        static void Main(string[] args)
        {
            TestFactory.Instance.Run();
        }
    }
}
