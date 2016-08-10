using System;
using ExtensionLibHelper;
using ExtensionLibHelper.FileLibHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtensionLibHelperUnitTest
{
    [TestClass]
    public class FileLibHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //测试Ini读取
            var ini = new IniHelper("C:\\CodingWorld\\1Configuration.ini");
            ini.Write("section", "test", "你麻痹","的");
            var tes = ini.ReadValue("test", "section");
            Console.WriteLine(tes);
        }
    }
}
