using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views;

public partial class DetallePastelPage : ContentPage
{
	public DetallePastelPage()
	{
		InitializeComponent();
		BindingContext = new DetallePastelViewModel();
	}

    private async void OnExpandImageTapped(object sender, TappedEventArgs e)
    {
        var url = e.Parameter as string;
        if (!string.IsNullOrWhiteSpace(url))
        {
            await Navigation.PushModalAsync(new ImagenLightboxPage(url), true);
        }
    }
}
