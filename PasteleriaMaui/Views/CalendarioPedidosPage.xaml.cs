using System.Linq;
using PasteleriaMaui.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PasteleriaMaui.Views;

public partial class CalendarioPedidosPage : ContentPage
{
    private readonly TransaccionesService _transaccionesService;

    public CalendarioPedidosPage()
    {
        InitializeComponent();
        _transaccionesService = new TransaccionesService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarCalendario();
    }

    private async Task CargarCalendario()
    {
        ContenedorCalendario.Children.Clear();
        var pedidos = await _transaccionesService.GetPedidosAsync(null);

        var entregasAgrupadas = pedidos
            .Where(p => p.Estatus != "Entregado" && p.Estatus != "Cancelado")
            .GroupBy(p => p.FechaEntrega.Date)
            .OrderBy(g => g.Key)
            .ToList();

        if (!entregasAgrupadas.Any())
        {
            ContenedorCalendario.Children.Add(new Label { Text = "No hay entregas pendientes.", FontSize = 18, TextColor = Color.FromArgb("#5C4033"), HorizontalOptions = LayoutOptions.Center });
            return;
        }

        foreach (var grupo in entregasAgrupadas)
        {
            var headerLabel = new Label
            {
                Text = $"📅 {grupo.Key.ToString("dddd, dd de MMMM yyyy").ToUpper()}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#5C4033"),
                Margin = new Thickness(0, 10, 0, 5)
            };
            ContenedorCalendario.Children.Add(headerLabel);

            foreach (var pedido in grupo)
            {
                var card = new Frame
                {
                    CornerRadius = 8,
                    BorderColor = Color.FromArgb("#D2B48C"),
                    BackgroundColor = Color.FromArgb("#FFF8DC"),
                    Padding = 15,
                    Margin = new Thickness(0, 0, 0, 10),
                    HasShadow = false
                };

                var stack = new VerticalStackLayout { Spacing = 5 };
                stack.Children.Add(new Label { Text = $"👤 Cliente: {pedido.Usuario?.Nombre ?? "Desconocido"}", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#8B4513") });
                stack.Children.Add(new Label { Text = $"🎂 Pastel: {pedido.Pastel?.Nombre} ({pedido.Cantidad} ud)", FontSize = 14, TextColor = Color.FromArgb("#4E342E") });
                stack.Children.Add(new Label { Text = $"📌 Estatus: {pedido.Estatus}", FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#E65100") });

                card.Content = stack;
                ContenedorCalendario.Children.Add(card);
            }
        }
    }
}
