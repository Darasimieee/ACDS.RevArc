using System;
using System.Net;
using ACDS.RevBill.Entities;

namespace ACDS.RevBill.Helpers
{
    public class SmsUtility
    {
        public MessageQueue SendSMS(MessageQueue messageQueue)
        {
            try
            {
                using (var client = new WebClient()) //WebClient  
                {
                    client.Headers.Add("Content-Type:application/json"); //Content-Type  
                    client.Headers.Add("Accept:application/json");
                    client.QueryString.Add("username", "alphabeta");
                    client.QueryString.Add("password", "decay.81$");
                    client.QueryString.Add("sender", "LASG");
                    client.QueryString.Add("message", messageQueue.Message);
                    client.QueryString.Add("recipient", messageQueue.Phone);
                    client.QueryString.Add("corporate", "1");

                    DateTime dt = DateTime.Now;

                    string queryBalance = client.DownloadString("https://api.loftysms.com/simple/getbalance");

                    decimal Balance = Convert.ToDecimal(queryBalance);

                    if (Balance > 2)
                    {
                        var result = client.DownloadString("https://api.loftysms.com/simple/sendsms"); //URI  
                    }
                    return messageQueue;
                }
            }

            catch (Exception Ex)
            {

            }
            return messageQueue;
        }
    }
}

