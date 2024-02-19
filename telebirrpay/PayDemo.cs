using System.Net;
using System.Text;

namespace telebirrpay
{
    public class PayDemo
    {
        public void Demo() {
            string appId = "e21eeacc6c544a56a6c10348b2a8f6e0";
            string url = "http://196.188.120.3:11443/ammapi/payment/service-openup/toTradeWebPay";
            string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqepmQo/qGJKhQwQucN//r+9bZEFBpNSuu7FwUFlN+VgchMgFIDR8rc22pZ2MBR/F+b5hYDaSBeQjtLz6LxCzjw751IegfQldcKoQLBrnJu31ifAZZYQVE22BAq2qJx9By/7WrDDqH/lXTOLpOKvN7rGtyUyrdiHqoHKY1yadq/NU+mE/GVLzeu3waKfJkBYStzdX3aLY8Qu8+hcU1u/pAsPUoEZSIMJxCypDxFOigyzX4KM07mZ0iu59SPYrGBr7TQ12JxahgPde5GiSUQ6sIwcff+yCl5HEYPcEfCtaeeU0ZTOrGwy8BX9c1cBaYqS0FRpCa7zRsLX6O2TqBfUWCQIDAQAB";
            string ussdjson = "{\"appId\":\"e21eeacc6c544a56a6c10348b2a8f6e0\",\"nonce\":\"df2614e624414020b9062f9512cd55d9\",\"notifyUrl\":\"http://localhost/notifyUrl\",\"outTradeNo\":\"202110181634541567788\",\"receiveName\":\"Org Name\",\"returnUrl\":\"http://localhost/returnUrl\",\"shortCode\":\"9000\",\"subject\":\"Goods Name\",\"timeoutExpress\":\"30\",\"timestamp\":\"1634541567060\",\"totalAmount\":\"10\"}";
            string stringA = this.GenerateStringA(ussdjson);
            Console.WriteLine("stringA: " + stringA);

           string sing =  SHAHelper.GetSign(stringA);
           Console.WriteLine("sing: " + sing);

            string ussd = RSAHelper.EncryptionByPublicKey(ussdjson, publicKey);
            Console.WriteLine("ussd: " + ussd + "\r\n");

            //========== send http request ==========//
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"appId\":");
            sb.Append($"\"{appId}\"");
            sb.Append(",");
            sb.Append("\"sign\":");
            sb.Append($"\"{sing}\"");
            sb.Append(",");
            sb.Append("\"ussd\":");
            sb.Append($"\"{ussd}\"");
            sb.Append("}");
            Console.WriteLine("param: " + sb.ToString());
            string ret = HttpClientUtil.HttpPost(url, sb.ToString());
            Console.WriteLine(ret + "\r\n");

            //========== Decrypt ==========//
            string encryptData = "o7C3bTvsgr/xoSueBmgOZZuBFjA07xbKQqyokpk79cNCxvnbbfNLn3gxWy/X8GbexdesjW9q8B+QmhoVypzxtVe7iU3LzEOi1cZJpU5ISx93GJoEZeR9HwBuv+L0aidDxyxeBJCmFQqpRWKbGzMDdbJ3SnGyGU1o926AOh1pQiIEslmcjqUclVK70ev6GI8KuQbif5SCK4Nwg+g2ovVHWi523OLnOgrqJdRJ31cWkiB+Z9VjhsFWvcAJ8C1hHhpJtwqJoseS1kZa26gqllOfkZOrG+q0Z9Tf7MahQhY9oEevDSCAlAfdql4Aa4A9Zxpt/krrnrM0Lu8Z0Vwn2beO+1mr6XX6DBX67coY5jXZtGcg+Q6Z+bhdoPRiJ8eIiSRY6gpBi+gRnfhN0z3bCwYVVlkavN4lKQHCO1sllyLxo4aASO69KBu2yVHHock8jFwKWBXVPWe711mjrj6d1v9uAL9GpeLBS7pFKoy7VpEugfV3khHLQpy3/4zuCU3EDmJsMet6HL5ICazc8BZLHokgBqKkhvdDw99ElzJn9LqVyWmobAXlA6Rk15tinT2vk32i27fjSbkIirKKQuI+ppOwWXtcXAAfUZZN5dwCLGGeCfxN43+42mRTWqRnKgpoNB3500K0hG7iGW36F8rVld7vJY3N79g8qGQNNZO6OIbOkFU=";
            //string encryptData = "j9ymSFhZBU5QAtnxSUwx+3OKuL6sY66asOvwLHlCo4o6e8TMpFylKWu5reUbTyTFPCWBcsOax5lIsZerTSsSSlapjTa9kIuqfuMLXzJkf+jeISFo9qZ8+wIVQ83puHg5OcmkWooJp97ZkLfFLY1eOXCFgmoLWsBmKXp3KE8+htCEiWFTZ9PHyq+C/nemf0Y4c9aZvc1B4BgNYry3A78ZFxzyVzMRbpXOdhkmDhAWq+7AUkrNg3eNjNS/L+Afs3fogVx/Qg3by0oWrje/dTa9lYCVRn1cmLzNuKrGHFt1eh9IhHpgW+jym6x6fIHjh5kn9+u1XlyNpdQ5LW2OKMTktT/9TY7moQp9G9Wv5LzLy2EC3wx1gGPzmdlWBIjOTkA11WqCrtjY8m2IBD8QCzXCR9a9riRNS5j94UZCsPQzr+YJR8wePzu7ZNoaXpy3FUlpaN/TTNU5xdJxP5cDET9SZQq7Vh4Cp+iv1kdjPSDMcsV6DVRmOML5ikEDE1/TuenIByg+fjxORFAARcYXNaIdNHkLqqbNBV99GYKlb8t/WbjXKsYR/OUBP8JsnVvlQZw2NVUjhV3KILPwroxj5m97Eo5N8h242wi4UFW4aAZPQe4nB38EwooMN0NWjJRLqIIIR+A6B0AoXBVb+qcCQh+cosfY4vkxR0238ZlqOn8kGOslWIUPIEegaBF+VfdE7Db79FA/lwJAME2JKgsJi7PhO5qaYNOMwW57JOQvum/J86oyjVwDpXtpnDDVqKagGA2Xc0DQosTgG7nALdqARgl0jx3I9T90gWechazP3hMVkA1AmZwzy4HMN8NHgBbR6Bh3vtvTmNJtsn0gc60Pqo3JQyzNXJ0LfEfTIDvLuBISyf/cu6TlFARfOsROzq/3y6CKS7+A+21a7DBGqKpVQLGTTP6dI1xWtRA/h0+18x83mwQGAm7WYhs74pD7dsjKCqPF8K7xcNH5Bw8ZQS8Y0xActvgwI/7NRUjgb970OdnYHLOfDLyGEx7FJ7MXOrZdyKve";
            string decryptdate = RSAHelper.DecryptByPublicKey(encryptData, publicKey);
            Console.WriteLine("DecryptByPublicKey: " + decryptdate + "\r\n");

        }

        public string GenerateStringA(string paramJsonStr)
        {
            Dictionary<string, string> paramsMap = paramJsonStr.Trim().Split(',').ToDictionary(s => s.Split(':')[0].Replace("\"", ""), s => s.Split(':')[1].Replace("\"", ""));
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                object pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }

            String stringA = str.ToString().Substring(0, str.ToString().Length - 1);
            return stringA;
        }

    }
}
