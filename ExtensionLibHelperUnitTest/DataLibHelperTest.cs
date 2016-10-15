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
            sqlhelper.ConString = "Server=localhost;Database=HomeKit; User=root;Password=;Use Procedure Bodies=false;Charset=utf8;Allow Zero Datetime=True; Pooling=false; Max Pool Size=50;";
            var newtest=sqlhelper.ExecuteDataTable(CommandType.Text, "select * from systemconfiguration");
        }
    }
}
