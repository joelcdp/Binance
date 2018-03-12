using DayTrade;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AlertAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Now;
            var engine = new AlertEngine();
            while (true) engine.Run(start);
        }


    }
}
