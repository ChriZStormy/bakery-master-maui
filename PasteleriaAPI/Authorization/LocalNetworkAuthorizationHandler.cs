using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace PasteleriaAPI.Authorization
{
    public class LocalNetworkAuthorizationHandler : AuthorizationHandler<LocalNetworkRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocalNetworkAuthorizationHandler> _logger;

        public LocalNetworkAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ILogger<LocalNetworkAuthorizationHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LocalNetworkRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("No se pudo obtener HttpContext para validación de red local.");
                context.Fail();
                return Task.CompletedTask;
            }

            var remoteIp = httpContext.Connection.RemoteIpAddress;
            if (remoteIp == null)
            {
                _logger.LogWarning("No se pudo determinar la IP remota.");
                context.Fail();
                return Task.CompletedTask;
            }

            // Map IPv6-mapped IPv4 addresses to IPv4
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            var allowedRanges = _configuration.GetSection("AdminNetworkPolicy:AllowedRanges")
                .Get<string[]>() ?? new[]
                {
                    "127.0.0.0/8",    // Localhost
                    "10.0.0.0/8",     // Private Class A
                    "192.168.0.0/16", // Private Class C
                    "172.16.0.0/12",  // Private Class B
                    "100.64.0.0/10"   // CGNAT / Tailscale
                };

            // Always allow IPv6 loopback
            if (IPAddress.IsLoopback(remoteIp))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Allow IPv6 local/private network addresses (link-local and unique local fc00::/7)
            if (remoteIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                if (remoteIp.IsIPv6LinkLocal || remoteIp.IsIPv6SiteLocal || (remoteIp.GetAddressBytes()[0] & 0xFE) == 0xFC)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            foreach (var range in allowedRanges)
            {
                if (IsInRange(remoteIp, range))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            _logger.LogWarning(
                "Acceso admin denegado desde IP externa: {RemoteIp}. Usuario: {User}",
                remoteIp,
                context.User?.Identity?.Name ?? "desconocido");

            context.Fail();
            return Task.CompletedTask;
        }

        private static bool IsInRange(IPAddress address, string cidr)
        {
            var parts = cidr.Split('/');
            if (!IPAddress.TryParse(parts[0], out var network))
                return false;

            int prefixLength = parts.Length > 1 ? int.Parse(parts[1]) : 32;

            var networkBytes = network.GetAddressBytes();
            var addressBytes = address.GetAddressBytes();

            if (networkBytes.Length != addressBytes.Length)
                return false;

            int fullBytes = prefixLength / 8;
            int remainingBits = prefixLength % 8;

            for (int i = 0; i < fullBytes; i++)
            {
                if (networkBytes[i] != addressBytes[i])
                    return false;
            }

            if (remainingBits > 0 && fullBytes < networkBytes.Length)
            {
                int mask = 0xFF << (8 - remainingBits);
                if ((networkBytes[fullBytes] & mask) != (addressBytes[fullBytes] & mask))
                    return false;
            }

            return true;
        }
    }
}
