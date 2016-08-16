using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensionLibHelper.NetLibHelper;

namespace ExtensionLibHelperUnitTest
{
    /// <summary>
    /// NetLibHelperTest 的摘要说明
    /// </summary>
    [TestClass]
    public class NetLibHelperTest
    {
        public NetLibHelperTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO:  在此处添加测试逻辑
            //测试访问HTTP
            HttpHelper a = new HttpHelper();
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://10.129.8.29:8066/default.aspx?pg=6b04184a-3c96-43a1-b966-aa577df4effd",//URL     必需项
                Encoding = null,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                                //Encoding = Encoding.Default,
                Method = "get",//URL     可选项 默认为Get
                Timeout = 100000,//连接超时时间     可选项默认为100000
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                Cookie = "",//字符串Cookie     可选项
                UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                ContentType = "text/html",//返回类型    可选项有默认值
                //Referer = "http://www.sufeinet.com",//来源URL     可选项
                Allowautoredirect = true,//是否根据３０１跳转     可选项
               // CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
                Connectionlimit = 1024,//最大连接数     可选项 默认为1024
                //Postdata = "C:\\PERKYSU_20121129150608_ScrubLog.txt",//Post数据     可选项GET时不需要写
                PostDataType = PostDataType.FilePath,//默认为传入String类型，也可以设置PostDataType.Byte传入Byte类型数据
                //ProxyIp = "192.168.1.105：8015",//代理服务器ID 端口可以直接加到后面以：分开就行了    可选项 不需要代理 时可以不设置这三个参数
                //ProxyPwd = "123456",//代理服务器密码     可选项
                //ProxyUserName = "administrator",//代理服务器账户名     可选项
                ResultType = ResultType.Byte,//返回数据类型，是Byte还是String
                PostdataByte = System.Text.Encoding.Default.GetBytes("测试一下"),//如果PostDataType为Byte时要设置本属性的值
                CookieCollection = new System.Net.CookieCollection(),//可以直接传一个Cookie集合进来
            };
            //item.Header.Add("测试Key1", "测试Value1");
            //item.Header.Add("测试Key2", "测试Value2");
            //得到HTML代码
            HttpResult result = http.GetHtml(item);
            //取出返回的Cookie
            string cookie = result.Cookie;
            //返回的Html内容
            string html = result.Html;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //表示访问成功，具体的大家就参考HttpStatusCode类
            }
            //表示StatusCode的文字说明与描述
            string statusCodeDescription = result.StatusDescription;
        }
        

    }
}
