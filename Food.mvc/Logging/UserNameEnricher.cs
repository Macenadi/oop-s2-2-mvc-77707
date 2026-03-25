using Serilog.Core;
using Serilog.Events;

namespace Food.mvc.Logging
{
    public class UserNameEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";
            var property = propertyFactory.CreateProperty("UserName", userName);
            logEvent.AddPropertyIfAbsent(property);
        }
    }
}