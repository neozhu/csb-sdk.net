namespace CSBSDK.NET
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Linq;
    public class HttpCaller
    {
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        protected static FormUrlEncodedContent createformdata(Dictionary<string, string> ps) {
            var list = ps.ToList<KeyValuePair<string, string>>();
            var data = new FormUrlEncodedContent(list);
            return data;

        }

        protected static string createPost(Dictionary<string, string> ps)
        {
           
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in ps)
            {
             
                    builder.Append(string.Format("{0}={1}&", pair.Key, pair.Value));
            }
            string str = builder.ToString();
            if (str.EndsWith("&"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return  str;
        }

        public static string doPost(string requestURL, string apiName, string version, Dictionary<string, string> paramsMap, string accessKey, string securityKey)
        {
            validateParams(apiName, accessKey, securityKey);
            Dictionary<string, string> dictionary = parseUrlParamsMap(requestURL);
            if (paramsMap != null)
            {
                foreach (KeyValuePair<string, string> pair in paramsMap)
                {
                    if (!dictionary.ContainsKey(pair.Key))
                    {
                        dictionary[pair.Key] = pair.Value;
                    }
                }
            }
            requestURL = trimUrl(requestURL);
            Dictionary<string, string> ps = newParamsMap(dictionary, apiName, version, accessKey, securityKey);
            Dictionary<string, string> httpheaderkeys = new Dictionary<string, string>
            {
                {
                      "_api_name",
                     ps["_api_name"]
                },
                {
                      "_api_timestamp",
                     ps["_api_timestamp"]
                },
                {
                      "_api_access_key",
                     ps["_api_access_key"]
                },
                {
                      "_api_signature",
                     ps["_api_signature"]
                },
                {
                    "_api_version",
                    ps["_api_version"]
                }

            };

            string parameters = createPost(ps);
            //var content = createformdata(ps);
            return HttpPOST(requestURL, parameters, httpheaderkeys);
        }

        public static string Http(string url,  string parameters, Encoding charset, NetworkCredential _credentials, Dictionary<string, string> keys, string ContentType, string methed)
        {
            HttpWebRequest request = null;
            HttpWebResponse response;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpCaller.CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Method = methed;
            request.ContentType = ContentType;
            request.Accept = "application/x-www-form-urlencoded; charset=UTF-8";
            if (keys != null)
            {
                foreach (KeyValuePair<string, string> pair in keys)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }
            }
            if (_credentials != null)
            {
                request.Credentials = _credentials;
            }
            if ((parameters != null) && (parameters.Length > 0))
            {
                byte[] bytes = charset.GetBytes(parameters.ToString());
                request.ContentLength = bytes.Length;
                using (Stream stream2 = request.GetRequestStream())
                {
                    //body.CopyToAsync(stream2);
                    stream2.Write(bytes, 0, bytes.Length);
                }
            }
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException exception)
            {
                response = (HttpWebResponse) exception.Response;
            }
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return str;
        }
        private static Dictionary<string, string> httpHeadParametersSet(string apiName, string version, string accessKey, string securityKey,Dictionary<string,string> requestdata) {
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("_api_name", apiName);
            paramsDic.Add("_api_version", version);
       
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (TimeSpan)(DateTime.UtcNow - time);
            paramsDic.Add("_api_timestamp", ((long)span.TotalMilliseconds).ToString());
            paramsDic.Add("_api_access_key", accessKey);

            paramsDic.Add("_api_signature", SignUtil.sign(requestdata, securityKey));

            return paramsDic;

        }
        public static string HttpPOST(string url,  string parameters, Dictionary<string, string> keys)
        {
            return Http(url,  parameters, Encoding.GetEncoding("utf-8"), null, keys, "application/x-www-form-urlencoded", "POST");
        }

        private static Dictionary<string, string> newParamsMap(Dictionary<string, string> paramsMap, string apiName, string version, string accessKey, string securityKey)
        {
            Dictionary<string, string> newParamsMap = new Dictionary<string, string>();
            if (paramsMap != null)
            {
                foreach (KeyValuePair<string, string> pair in paramsMap)
                {
                    if (!newParamsMap.ContainsKey(pair.Key))
                    {
                        newParamsMap[pair.Key] = pair.Value;
                    }
                }
            }
            newParamsMap.Add("_api_access_key", accessKey);
            newParamsMap.Add("_api_name", apiName);
          
               
          
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - time);
            newParamsMap.Add("_api_timestamp", ((long) span.TotalMilliseconds).ToString());
            
                
            
            newParamsMap.Add("_api_version", version);
            newParamsMap.Add("_api_signature", SignUtil.sign(newParamsMap, securityKey));
            return newParamsMap;
        }

        private static Dictionary<string, string> parseUrlParamsMap(string requestURL)
        {
            bool flag = requestURL.IndexOf("?") > -1;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (flag)
            {
                int index = requestURL.IndexOf("?");
                char[] separator = new char[] { '&' };
                string[] strArray = requestURL.Substring(index + 1).Split(separator);
                foreach (string str2 in strArray)
                {
                    index = str2.IndexOf("=");
                    if (index <= 0)
                    {
                        throw new Exception("bad request URL, url params error:" + requestURL);
                    }
                    dictionary.Add(str2.Substring(0, index), str2.Substring(index + 1));
                }
            }
            return dictionary;
        }

        private static string trimUrl(string requestURL)
        {
            int index = requestURL.IndexOf("?");
            string str = requestURL;
            if (index >= 0)
            {
                str = requestURL.Substring(0, index);
                Console.WriteLine("-- orignal url=" + requestURL);
                Console.WriteLine("-- new url=" + str);
            }
            return str;
        }

        private static void validateParams(string apiName, string accessKey, string securityKey)
        {
            if (apiName == null)
            {
                throw new Exception("param apiName can not be null!");
            }
            if ((accessKey != null) && (securityKey == null))
            {
                throw new Exception("param securityKey can not be null for a given accessKey!");
            }
        }
    }
}

