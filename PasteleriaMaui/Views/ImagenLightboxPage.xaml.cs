namespace PasteleriaMaui.Views;

public partial class ImagenLightboxPage : ContentPage
{
    public ImagenLightboxPage(string imageUrl)
    {
        InitializeComponent();

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            ImagenCompleta.Source = imageUrl;
        }
    }

    private async void OnCerrarClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(true);
    }
}
