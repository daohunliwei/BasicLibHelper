using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensionLibHelper.DataLibHelper;
using System.Data;

namespace ExtensionLibHelperUnitTest
{
    [TestClass]
    public class DataLibHelperTest
    {
        [TestMethod]
        public void TestMethodMySQL()
        { 
            MySQLHelper sqlhelper = new MySQLHelper(); 
            sqlhelper.ConString = "Server=192.211.55.2;Database=SpiderMan; User=daodao;Password=doyouloveme123!;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;";
            var newtest=sqlhelper.ExecuteDataTable(CommandType.Text, "select * from Test");
        }
    }
}
