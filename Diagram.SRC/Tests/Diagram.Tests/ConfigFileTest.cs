using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diagram
{
    [TestClass]
    public class ConfigFileTest
    {
        [TestMethod]
        public void CreateConfigurationFile()
        {
            ConfigFile configFile = new ConfigFile("configuration");

            configFile.Set("item1/item2/item3/item4", "value1");
            configFile.Set("item1/item2/item5", "value2");
            configFile.Set("item1/item2/item3/item6", "value3");
            configFile.Set("item7", "value4");
            configFile.Set("item1/item2/item5", "value5");

            configFile.SaveFile("test.xml");

            string xml = configFile.Save();
            configFile.Open(xml);

            Assert.IsTrue(configFile.Get("item1/item2/item5") == "value5");
        }
    }
}
