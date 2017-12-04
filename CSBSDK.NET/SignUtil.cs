namespace CSBSDK.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;

    public class SignUtil
    {
        public static string sign(Dictionary<string, string> newParamsMap, string secretKey)
        {
            Dictionary<string, string> dictionary = (from d in newParamsMap
                
                select d).ToDictionary<KeyValuePair<string, string>, string, string>(k => k.Key, v => v.Value);
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
              
                builder.Append(string.Format("{0}={1}&", pair.Key, pair.Value));
            }
            string str = builder.ToString();
            if (str.EndsWith("&"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            Console.WriteLine(str);
            HMACSHA1 hmacsha = new HMACSHA1 {
                Key = Encoding.UTF8.GetBytes(secretKey)
            };
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(hmacsha.ComputeHash(bytes));
        }

        
    }
}

