using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var requesturl = "https://api-csb-broker.boe.com.cn:443/test";
            var apiname = "boe_hubtoecc_gr";
            var version = "1.0.0";

            var xd = new XmlDocument();
            xd.Load(@"d:\\GR-FL11IN0000082266.xml");
            var js = JsonConvert.SerializeXmlNode(xd, Newtonsoft.Json.Formatting.None, true);


            var requestdata = new Dictionary<string, string>();
            requestdata.Add("MT_BOE_HUBTOECC_GR",  js );
            var ak = "94717a0403804b039834a7cab929d4a4";
            var sk = "zJke7U3yGMG7kGQ9SfxfU/hzzBg=";

            var result = CSBSDK.NET.HttpCaller.doPost(requesturl, apiname, version, requestdata, ak, sk);
            Console.Write(result);

            Console.ReadLine();

            
        }
    }
}
