# csb-sdk.net
HTTP SDK工具类，用来向服务端发送HTTP请求，请求支持POST/GET方式。如果提供了AccessKey和SecurityKey参数信息，它能够在内部将请求消息进行签名处理，然后向CSB服务端发送进行验证和调用。

# HTTP Client SDK 使用方式
### C# 代码
``` javascript
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
```


# 签名机制的说明
如果CSB 控制台发布出来的HTTP 服务声明需要鉴权处理，则客户端调用该服务试CSB 要对每个访问请求进行身份验证。这就要求客户端无论使用HTTP 还是HTTPS协议提交请求，都需要在请求中包含签名（Signature）信息。 CSB通过使用Access Key ID 和Access Key Secret进行对称加密的方法来验证请求的发送者身份。 Access Key ID 和Access Key Secret由在管理控制台在服务订购时候指定和确认，HTTP SDK在访问时，按照下面的方法对请求进行签名处理：

1 使用请求参数构造规范化的请求字符串（Canonicalized Query String）。 a. 按照参数名称的字典顺序对请求中所有的请求参数，包括上文中中描述的“公共请求参数”（但不包括_api_signature 参数本身）和给定了的请求接口的自定义参数进行排序。 说明：当使用GET方法提交请求时，这些参数就是请求URI中的参数部分（即URI 中“?”之后由“&”连接的部分）。

b. 参数名称和值使用英文等号（=）进行连接。再把英文等号连接得到的字符串按参数名称的字典顺序依次使用&符号连接，即得到规范化请求字符串。 注意：请求参数是原始的name-value，即不能进行URL Encode等操作。

2 按照RFC2104 的定义，使用上述用于签名的字符串计算签名HMAC 值。注意：计算签名时使用的Key 就是用户持有的SecretKey，使用的哈希算法是SHA1。

3 按照Base64编码规则把上面的HMAC值编码成字符串，即得到签名值（Signature）。

4 将得到的签名值作为_api_signature参数添加到请求参数中，即完成对请求签名的过程。

### HTTP SDK 签名处理的图示
![](https://github.com/aliyun/csb-sdk/raw/master/http-client/img/http-sign.png)
