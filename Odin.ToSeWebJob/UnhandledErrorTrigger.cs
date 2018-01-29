using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;

namespace Odin.ToSeWebJob
{
    public class UnhandledErrorTrigger : IDisposable
    {
        private readonly TelemetryClient _telemetryClient;

        public UnhandledErrorTrigger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void UnHandledException([ErrorTrigger("0:01:00", 4)] TraceFilter filter, TextWriter log)
        {
            foreach (var traceEvent in filter.GetEvents())
            {
                _telemetryClient.TrackException(traceEvent.Exception);
            }

            // log the last detailed errors to the Dashboard
            log.WriteLine(filter.GetDetailedMessage(1));
        }

        public void Dispose()
        {
            _telemetryClient.Flush();
        }
    }
}
