using System.Security.Claims;

namespace ChicaizaSuite.Api.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetTenantId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                // Fallback for demo/seeder or unauthenticated requests.
                // In production, this might throw an exception if strict multi-tenancy is required.
                return 1;
            }

            var tenantClaim = user.FindFirst("TenantId");
            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
            {
                return tenantId;
            }

            return 1; // Default tenant
        }
    }
}
