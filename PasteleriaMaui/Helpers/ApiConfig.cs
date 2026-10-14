namespace PasteleriaMaui.Helpers
{
    public static class ApiConfig
    {
        // Endpoint base para consumo de la API REST.
        // Si se desea ejecutar contra la API local de desarrollo, puede cambiarse a "http://localhost:5105/" o "https://localhost:7022/"
        public static string BaseUrl { get; set; } = "https://dolcevita-guh8gshvd0dre8d3.mexicocentral-01.azurewebsites.net/";
    }
}
