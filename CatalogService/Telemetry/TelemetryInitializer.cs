using Microsoft.ApplicationInsights.Channel;

namespace CatalogService.Telemetry
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var context = System.Diagnostics.Activity.Current;

            if (context != null)
            {
                telemetry.Context.Operation.Id = context.RootId;
                telemetry.Context.Operation.ParentId = context.ParentId;
                telemetry.Context.Operation.CorrelationVector = context.Id;
            }
        }
    }
}
