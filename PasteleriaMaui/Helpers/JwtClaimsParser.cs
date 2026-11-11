using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PasteleriaMaui.Helpers
{
    public static class JwtClaimsParser
    {
        /// <summary>
        /// Parses a JWT token's payload (without cryptographic validation — that's the server's job)
        /// and returns key claims: Id, Name, Email, Role.
        /// </summary>
        public static Models.Usuario? ParseToken(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3)
                    return null;

                var payload = parts[1];
                // Add padding if needed
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var jsonBytes = Convert.FromBase64String(
                    payload.Replace('-', '+').Replace('_', '/'));
                var json = Encoding.UTF8.GetString(jsonBytes);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var usuario = new Models.Usuario();

                if (root.TryGetProperty("id", out var idProp))
                    usuario.Id = idProp.GetString() ?? string.Empty;

                if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", out var nameProp))
                    usuario.Nombre = nameProp.GetString() ?? string.Empty;

                if (root.TryGetProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", out var emailProp))
                    usuario.Email = emailProp.GetString() ?? string.Empty;

                if (root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var roleProp))
                {
                    if (roleProp.ValueKind == JsonValueKind.Array)
                    {
                        var roles = roleProp.EnumerateArray().Select(r => r.GetString()).ToList();
                        usuario.Rol = roles.Contains("Admin") ? "Admin" : (roles.FirstOrDefault() ?? "Cliente");
                    }
                    else
                    {
                        usuario.Rol = roleProp.GetString() ?? "Cliente";
                    }
                }

                return usuario;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a JWT token is expired.
        /// </summary>
        public static bool IsTokenExpired(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3) return true;

                var payload = parts[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var jsonBytes = Convert.FromBase64String(
                    payload.Replace('-', '+').Replace('_', '/'));
                var json = Encoding.UTF8.GetString(jsonBytes);

                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("exp", out var expProp))
                {
                    var expUnix = expProp.GetInt64();
                    var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix);
                    return expDate <= DateTimeOffset.UtcNow;
                }

                return true;
            }
            catch
            {
                return true;
            }
        }
    }
}
