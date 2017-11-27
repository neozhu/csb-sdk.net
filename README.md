# csb-sdk.net
The CSB-SDK is a client-side invocation SDK for HTTP or Web Service API opened by the CSB (Cloud Service Bus) product. It is responsible for invoking the open API and signing the request information.

~~~ javascript
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
            Console.Write(result);

            
        }
    }
~~~
