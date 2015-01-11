using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sinch.SMS;

namespace SendSMS_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            
            SendSMS();
            Console.ReadKey(true);
            
        }

        static async void SendSMS()
        {
            var client = new Sinch.SMS.Client("yourkey", "yoursecret");
            var messageid = await client.SendSMS("+46722223355", "hello from sinch");
            Console.WriteLine("message id:" + messageid);
            var result = await client.CheckStatus(messageid);
            Console.WriteLine("Message status:" + result);
            while (result == SNSStatus.Pending)
            {
                await Task.Delay(1500);
                result = await client.CheckStatus(messageid);
                Console.WriteLine("Message status:" + result);
            }
            Console.WriteLine("Press any key  to quit");
            
        }
    }
}
