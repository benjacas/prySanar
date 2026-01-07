namespace pryPrueba;

public partial class frmMiSesion : ContentPage
{
	public frmMiSesion()
	{
		InitializeComponent();
	}

    Border _selectedCard;
    string _dificultadSeleccionada;

    async Task SelectCard(Border card, Color color, string dificultad)
    {
        // Desmarcar la anterior
        if (_selectedCard != null)
        {
            _selectedCard.BackgroundColor = Colors.Transparent;
            _selectedCard.StrokeThickness = 1;
            await _selectedCard.ScaleTo(1, 80);
        }

        // Marcar la nueva
        card.BackgroundColor = color.WithAlpha(0.15f);
        card.StrokeThickness = 2;
        await card.ScaleTo(0.97, 80);
        await card.ScaleTo(1, 80);

        _selectedCard = card;
        _dificultadSeleccionada = dificultad;

        BtnContinuar.IsEnabled = true;
        BtnContinuar.Opacity = 1;
    }

    private async void FacilTapped(object sender, EventArgs e)
    {
        await SelectCard(FacilCard, Color.FromArgb("#4CAF50"),"Facil");
    }

    private async void MedioTapped(object sender, EventArgs e)
    {
        await SelectCard(MedioCard, Color.FromArgb("#FF9800"),"Medio");
    }

    private async void DificilTapped(object sender, EventArgs e)
    {
        await SelectCard(DificilCard, Color.FromArgb("#F44336"),"Dificil");
    }

    private async void BtnContinuar_Clicked (object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_dificultadSeleccionada))
            return;

        await Navigation.PushAsync(
            new frmBurbuja(_dificultadSeleccionada)
        );
    }

}