using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var requesturl = "https://api-csb-broker.boe.com.cn:443/test";
            var apiname = "boe_hubtoecc_gr";
            var version = "1.0.0";
            var requestdata = new Dictionary<string, string>();
            requestdata.Add("MT_BOE_HUBTOECC_GR", "{\"REQUESTDATA\":{}}");
            var ak = "94717a0403804b039834a7cab929d4a4";
            var sk = "zJke7U3yGMG7kGQ9SfxfU/hzzBg=";

            var result = CSBSDK.NET.HttpCaller.doPost(requesturl, apiname, version, requestdata, ak, sk);

            
        }
    }
}
