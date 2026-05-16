using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views;

public partial class DetallePastelPage : ContentPage
{
	public DetallePastelPage()
	{
		InitializeComponent();
		BindingContext = new DetallePastelViewModel();
	}
}