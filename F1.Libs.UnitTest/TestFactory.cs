using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace F1.Libs.UnitTest
{
    public class TestFactory:ITestFactory
    {
        private static object _lockObj = new object();
        private static ITestFactory _instance = null;

        private TestConfig _config = null;

        private TestFactory()
        {
            _readConfig();
        }

        public static ITestFactory Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new TestFactory();
                    }

                    return _instance;
                }
            }
        }

        private void _readConfig()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "testconfig.dat";            
            using (var tr = new StreamReader(path))
            {
                _config = JsonConvert.DeserializeObject<TestConfig>(tr.ReadToEnd());
                tr.Close();
            }
        }

        public void Run()
        {
            var assembly = Assembly.LoadFrom(System.Reflection.Assembly.GetEntryAssembly().Location);
            var types = assembly.GetTypes().Where(a => a.BaseType != null && a.BaseType.FullName.StartsWith("F1.Libs.UnitTest.BaseUnitTest")
                && a.FullName != "F1.Libs.UnitTest.BaseUnitTest");
            foreach (var item in types)
            {
                if (_config.Excludes.Contains(item.Name))
                    continue;

                if (_config.Includes.Contains(item.Name) || _config.Excludes.Count == 0)
                {
                    var handler = Activator.CreateInstance(item) as IUnitTest;
                    handler.Execute();
                }
            }
        }
    }
}
