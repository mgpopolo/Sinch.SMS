using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.SMS
{
    public class Client
    {
        private string _applicationKey;
        private string _applicationSecret;
        private string baseURL = "https://messagingapi.sinch.com/v1/sms/";
        public Client()
        {
            throw new Exception("Client must be initialized with kay and secret");
        }
        public Client(string applicationKey, string applicationSecret)
        {
            _applicationKey = applicationKey;
            _applicationSecret = applicationSecret;
        }

        private string SignString(string stringtoSign)
        {
            var sha256 = new HMACSHA256(Convert.FromBase64String(_applicationSecret));
            var signature = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(stringtoSign)));
            return signature;
        }

        private string SignRequest(string httpMethod, string requestBody, string url, string timeStamp)
        {

            string tosign = httpMethod + "\n" +
                           Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(requestBody))) + "\n" +
                           "application/json; charset=utf-8\n" +
                           "x-timestamp:" + timeStamp + "\n" +
                           url;
            return _applicationKey + ":" + SignString(tosign);
        }
        private string SignGetRequest(string httpMethod, string url, string timeStamp)
        {

            string tosign = httpMethod + "\n" +
                            "\n" +
                           "\n" +
                           "x-timestamp:" + timeStamp + "\n" +
                           url;
            return _applicationKey + ":" + SignString(tosign);
        }

        public async Task<int> SendSMS(string number, string message)
        {
                var url = baseURL + number;
                var smsRequest = new SMSRequest { Message=message};
                var httpClient = new HttpClient();
                var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                httpClient.DefaultRequestHeaders.Add("x-timestamp", timestamp);
                httpClient.DefaultRequestHeaders.Add("Authorization", "application " + SignRequest("POST", JsonConvert.SerializeObject(smsRequest), "/v1/sms/" + number, timestamp));
                var response = await httpClient.PostAsJsonAsync(url, smsRequest);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<SMSResult>();
                return result.MessageId;
        }

        public async Task<SNSStatus> CheckStatus(int messageid)
        {
            var url = baseURL + messageid;
            
            var httpClient = new HttpClient();
            var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
            httpClient.DefaultRequestHeaders.Add("x-timestamp", timestamp);
            httpClient.DefaultRequestHeaders.Add("Authorization", "application " + SignGetRequest("GET", "/v1/sms/" + messageid, timestamp));
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<SMSStatusResult>();
            return result.Status;
        }

    }

    public enum SNSStatus
    {
        Unknown = 1,
        Pending = 2,
        Successful = 3,
        Faulted = 4
    }

    //private sealed class SMSStatus1
    //{

    //    private readonly String name;
    //    private readonly int value;

    //    public static readonly SMSStatus1 Unknown  = new SMSStatus1(1, "Unknown");
    //    public static readonly SMSStatus1 Pending = new SMSStatus1(2, "Pending");
    //    public static readonly SMSStatus1 Successful = new SMSStatus1(3, "Successful");
    //    public static readonly SMSStatus1 Faulted = new SMSStatus1(3, "Faulted");


    //    private SMSStatus1(int value, String name)
    //    {
    //        this.name = name;
    //        this.value = value;
    //    }

    //    public override String ToString()
    //    {
    //        return name;
    //    }

    //}
    public class SMSStatusResult
    {

        public SNSStatus Status { get; set; }
    }
}
