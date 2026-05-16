using System.Linq;
using PasteleriaMaui.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;

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
            ContenedorCalendario.Children.Add(new Label { Text = "No hay entregas pendientes.", FontSize = 16, TextColor = Color.FromArgb("#795548"), HorizontalOptions = LayoutOptions.Center });
            return;
        }

        foreach (var grupo in entregasAgrupadas)
        {
            var headerLabel = new Label
            {
                Text = $"📅 {grupo.Key.ToString("dddd, dd de MMMM yyyy").ToUpper()}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#3E2723"),
                Margin = new Thickness(0, 15, 0, 5)
            };
            ContenedorCalendario.Children.Add(headerLabel);

            foreach (var pedido in grupo)
            {
                var card = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                    Stroke = new SolidColorBrush(Color.FromArgb("#EAE4DB")),
                    StrokeThickness = 1,
                    BackgroundColor = Color.FromArgb("#FFFFFF"),
                    Padding = 20,
                    Margin = new Thickness(0, 0, 0, 15),
                    Shadow = new Shadow { Brush = new SolidColorBrush(Color.FromArgb("#8B5E34")), Offset = new Point(0, 4), Radius = 10, Opacity = 0.06f }
                };

                var stack = new VerticalStackLayout { Spacing = 6 };
                stack.Children.Add(new Label { Text = $"Cliente: {pedido.Usuario?.Nombre ?? "Desconocido"}", FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#5D4037") });
                
                var separator = new BoxView { HeightRequest = 1, Color = Color.FromArgb("#F5F0E6"), Margin = new Thickness(0, 4) };
                stack.Children.Add(separator);

                stack.Children.Add(new Label { Text = $"Pastel: {pedido.Pastel?.Nombre} ({pedido.Cantidad} ud)", FontSize = 15, TextColor = Color.FromArgb("#795548") });
                
                var estatusBorder = new Border
                {
                    BackgroundColor = Color.FromArgb("#FFF8F5"),
                    Stroke = new SolidColorBrush(Color.FromArgb("#FFCCBC")),
                    StrokeThickness = 1,
                    Padding = new Thickness(10, 5),
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 5, 0, 0),
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
                    Content = new Label { Text = pedido.Estatus, FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#D84315") }
                };
                stack.Children.Add(estatusBorder);

                card.Content = stack;
                ContenedorCalendario.Children.Add(card);
            }
        }
    }
}
