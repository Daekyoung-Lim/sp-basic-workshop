using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Timers;

namespace Question4_Server
{
    class ManagerHttpServer
    {
        class Report
        {
            public string managerId { get; set; }
            public string reportId { get; set; }
            private Timer timerExpire;

            Report(string managerId, string reportId)
            {
                this.managerId = managerId;
                this.reportId = reportId;
                timerExpire = new System.Timers.Timer();
                timerExpire.Interval = 5;
                timerExpire.Elapsed += new ElapsedEventHandler(timerHandler);
                timerExpire.Start();
            }

            private void timerHandler(object sender, ElapsedEventArgs e)
            {
                timerExpire.Stop();
            }
        }

        HttpListener listener = new HttpListener();
        Dictionary<string, Report> reportQueue;

        public ManagerHttpServer()
        {
            reportQueue = new Dictionary<string, Report>();

            listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:8081/");
            listener.Start();
        }

        public void DoHttpWork()
        {
            while (true)
            {
                var context = listener.GetContext();
                Console.WriteLine(string.Format("Request({0}) : {1}", context.Request.HttpMethod, context.Request.Url));

                if (context.Request.HttpMethod == "GET")
                {
                    string[] urlSegments = context.Request.Url.Segments;

                    string managerId = urlSegments[0];
                    string date = urlSegments[1];

                    //manager id, date
                    //response w report
                    //input wait queue with timer


                    

                    //make log
                }
                else if (context.Request.HttpMethod == "POST")
                {
                    //FINISH

                    //FAIL

                    //make log
                }
            }
        }
    }
}
