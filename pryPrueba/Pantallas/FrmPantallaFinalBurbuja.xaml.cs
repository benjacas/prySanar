using System.Threading.Tasks;

namespace pryPrueba.Pantallas;

public partial class PantallaFinalBurbuja : ContentPage
{
	public PantallaFinalBurbuja()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CirculoFinal.ScaleTo(1.2, 300, Easing.CubicOut);
        await CirculoFinal.ScaleTo(1.0, 150, Easing.CubicIn);
    }

    private async void BtnFinalizar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new frmMiSesion());
    }
}