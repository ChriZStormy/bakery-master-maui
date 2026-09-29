using PasteleriaMaui.Models;

namespace PasteleriaMaui
{
    public static class AppSession
    {
        public static Usuario CurrentUser { get; set; }
        public static string Token { get; set; }
        public static bool HasAuthError { get; set; }
    }
}
