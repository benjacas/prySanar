namespace pryPrueba;

public partial class frmListaEjercicios : ContentPage
{
	public frmListaEjercicios()
	{
		InitializeComponent();
	}

	//al tocar la tarjeta brubja se abre frmlMiSesion
	private async void CardBurbuja_Tapped(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new frmMiSesion());
	}

}