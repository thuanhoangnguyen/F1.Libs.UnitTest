using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1.Libs.UnitTest
{
    public sealed class TestData
    {
        public string Desc { get; set; }
        public List<TestDataItem> TestItems { get; set; }
        public Func<TestDataItem, bool> CompareAction { get; set; }

        internal void Compare()
        {
            foreach (var item in TestItems)
            {
                bool? result = null;
                if (CompareAction != null)
                {
                    result = CompareAction(item);
                }
                else
                {
                    result = string.IsNullOrEmpty(item.AOP) == string.IsNullOrEmpty(item.EOP);
                }

                item.Result = result;
            }
        }
    };

    public sealed class TestDataItem
    {        
        public string Input {get;set;}
        public string EOP {get;set;}
        public string AOP {get;set;}
        public bool? Result { get; set; }
    };
}
