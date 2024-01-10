using Microsoft.ApplicationInsights.Channel;

namespace CatalogService.Telemetry
{
    public interface ITelemetryInitializer
    {
        void Initialize(ITelemetry telemetry);
    }
}