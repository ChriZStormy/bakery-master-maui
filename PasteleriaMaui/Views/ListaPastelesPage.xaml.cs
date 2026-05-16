using PasteleriaMaui.ViewModels;

namespace PasteleriaMaui.Views;

public partial class ListaPastelesPage : ContentPage
{
	private ListaPastelesViewModel _viewModel;

	public ListaPastelesPage()
	{
		InitializeComponent();
		_viewModel = new ListaPastelesViewModel();
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		
        if (AppSession.CurrentUser?.Rol == "Cliente")
        {
            this.Title = "Menú";
            BtnRegistrarPastel.IsVisible = false;
        }
        else 
        {
            this.Title = "Lista de Pasteles";
            BtnRegistrarPastel.IsVisible = true;
        }

        await _viewModel.CargarPasteles();
	}
}