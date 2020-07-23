using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VLM.Data;

namespace VLM_DotNetCore.BackgroundServices
{
    public class FineService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        private string Schedule => "*/10 * * * * *"; //Runs every 10 seconds

        public FineService()
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if(now > _nextRun)
                {
                    Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while(!stoppingToken.IsCancellationRequested);
        }

        private void Process()
        {
            //Console.WriteLine("hello world " + DateTime.Now.ToString("F"));
        }
    }
}
