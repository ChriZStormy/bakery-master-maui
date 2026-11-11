namespace PasteleriaMaui.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = new();
    }
}
