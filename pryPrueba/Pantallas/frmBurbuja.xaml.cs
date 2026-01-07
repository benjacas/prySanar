namespace pryPrueba;

public partial class frmBurbuja : ContentPage
{
	string _dificultad;
	bool _animado = false;
	public frmBurbuja(string dificultad)
	{
        InitializeComponent();
        _dificultad = dificultad;
	}


	async Task Respirar() //Esto despues se puede modificar los segundos que se exhala e inhala para cambiar de dificultad, por ahora probemos si esto funciona
	{
		_animado = true;

		while (_animado)
		{
			//inhalar
			lblEstado.Text = "Inhalar";
			await Circulo.ScaleTo(1.4, 4000, Easing.SinInOut);

            if (!_animado) break;

            //Sostener
            lblEstado.Text = "Sostener";
			await Task.Delay(2000);

            if (!_animado) break;

            //Exhalar
            lblEstado.Text = "Exhalar";
			await Circulo.ScaleTo(1.0, 4000, Easing.SinInOut);
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_= Respirar();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _animado = false;
    }
}