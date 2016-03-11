using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F1.Libs.UnitTest
{
    public interface ITestReport
    {        
        void Render(List<TestData> TestCases);
    }
}
