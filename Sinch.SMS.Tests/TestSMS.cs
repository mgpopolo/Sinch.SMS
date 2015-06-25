using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sinch.SMS.Tests
{
    [TestClass]
    public class TestSMS
    {
        private string key = "";
        private string secret = "==";

        [TestMethod]
        public void SendSMSTestFrom()
        {

            var client = new Sinch.SMS.Client(key, secret);

            var messageid = client.SendSMS("+12023679434", "+46722223355", "hello from .net").Result;
            Assert.AreNotEqual(0, messageid);

        }

        [TestMethod]
        public void SendSMSTest() {

            var client = new Sinch.SMS.Client(key, secret);
            var messageid = client.SendSMS("+46722223355", "hello from .net").Result;
            Assert.AreNotEqual(0, messageid);

        }

        [TestMethod]
        public void CheckStatusSucess()
        {

            var client = new Sinch.SMS.Client(key, secret);
            var messageid = client.SendSMS("+46722223355", "hello from .net").Result;
            var status = client.CheckStatus(messageid).Result;
            Assert.AreEqual(SMSStatus.Successful, status);

        }
    }
}
