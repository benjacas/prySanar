namespace pryPrueba;
using Microsoft.Maui.Essentials;


public partial class frmBurbuja : ContentPage
{
    //dificultad seleccionada
	string _dificultad;

    //animado apagado
	bool _animado = false;

    //Temporizador
    TimeSpan _tiempoRestante = TimeSpan.FromMinutes(5);
    IDispatcherTimer _timerSesion;

    //Pausar - Reaundar
    bool _pausado = false;

    //para reanudar?
    string _faseActual = "";
    double _escalaActual = 1.0;

    public frmBurbuja(string dificultad)
	{
        InitializeComponent();
        _dificultad = dificultad;
	}


	async Task Respirar() //Esto despues se puede modificar los segundos que se exhala e inhala para cambiar de dificultad, por ahora probemos si esto funciona
	{
        //switch (_dificultad)
        //{
        //    case = "Facil":
                    
        //    break;
              
        //}
		_animado = true;

		while (_animado)
		{
            for (int i = 0;i < 3; i++)//for para que entre en un bucle la primer fase
            {
                //inhalar
                lblEstado.Text = "Inhalar";
                AnimarCirculo(_escalaActual, 1.4, 4000);
                _faseActual = "Inhalar";
                await ContarRespiracion(4);
                Circulo.BackgroundColor = Colors.Pink;
                //VibrarSuave();


                if (!_animado) break;

                //Sostener
                lblEstado.Text = "Sostener";
                _faseActual = "Sostener";
                await ContarRespiracion(2);
                Circulo.BackgroundColor = Colors.Green;
                //await VibrarDoble();

                if (!_animado) break;

                //Exhalar
                lblEstado.Text = "Exhalar";
                _faseActual = "Exhalar";
                AnimarCirculo(_escalaActual, 1.0, 4000);
                await ContarRespiracion(4);
                Circulo.BackgroundColor = Colors.Purple;


                if (!_animado) break;
                //VibrarSuave();
            }
        }
			
	}

    async Task IniciarDescanso()
    {
        //Descanso
        lblEstado.Text = "Descanso";
        _faseActual = "Descanso";
        Circulo.BackgroundColor = Colors.LightBlue;// dejamos el círculo quieto en su escala actual
        await ContarRespiracion(10);
    }
    async Task Temporizador()
    {

        _timerSesion = Dispatcher.CreateTimer();
        _timerSesion.Interval = TimeSpan.FromSeconds(1);

        _timerSesion.Tick += (s, e) =>
        {
            if (_pausado) return; //si está pausado, frena
            _tiempoRestante = _tiempoRestante.Subtract(TimeSpan.FromSeconds(1));

            lblTemporizador.Text = _tiempoRestante.ToString(@"mm\:ss");

            if (_tiempoRestante <= TimeSpan.Zero)
            {
                FinalizarSesion();
            }
        };
        _timerSesion.Start();
    }

    async void FinalizarSesion()
    {
        _timerSesion?.Stop();
        _animado = false;

        lblEstado.Text = "Sesion Finalizada";
        lblTiempo.Text = "";
        Circulo.ScaleTo(1, 200);

        await DisplayAlert("Bien hecho", "Completaste tu sesión de respiración", "OK"); //MENSAJE DE FINALIZACION


        // Opcional: volver atrás
        // await Navigation.PopAsync();
    }

    protected override void OnAppearing()
	{
		base.OnAppearing();

        _animado = true;
        _ = Respirar();
		Temporizador();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _animado = false;

        _timerSesion?.Stop();
    }

    async Task ContarRespiracion(int segundos)
    {
        for (int i = segundos; i > 0 && _animado; i--)
        {
            //  Espera activa mientras está pausado
            while (_pausado)
                await Task.Delay(50);

            lblTiempo.Text = i.ToString();

            lblTiempo.Scale = 1.2;
            await lblTiempo.ScaleTo(1.0, 200, Easing.CubicOut);

            //  Esperar 1 segundo, pero en partes
            int elapsed = 0;
            while (elapsed < 1000)
            {
                if (_pausado || !_animado)
                    return;

                await Task.Delay(50);
                elapsed += 50;
            }
        }
    }


    void BtnPausa_Clicked(object sender, EventArgs e)
    {
        if (_pausado)
            Reanudar();
        else
            Pausar();
    }

    void Pausar()
    {
        _pausado = true;

        Circulo.AbortAnimation("RespiracionAnim");

        BtnPausa.Text = "Reanudar";
        lblEstado.Text = "Pausado";
    }

    void Reanudar()
    {
        _pausado = false;
        BtnPausa.Text = "Pausar";

        // Reanudar animación según fase
        if (_faseActual == "Inhalar")
            AnimarCirculo(_escalaActual, 1.4, 2000);

        else if (_faseActual == "Exhalar")
            AnimarCirculo(_escalaActual, 1.0, 2000);
    }


    void AnimarCirculo(double desde, double hasta, uint duracion)
    {
        var anim = new Animation(v =>
        {
            _escalaActual = v;
            Circulo.Scale = v;
        }, desde, hasta);

        anim.Commit(
            Circulo,
            "RespiracionAnim",
            length: duracion,
            easing: Easing.SinInOut
        );
    }

    void VibrarSuave()
    {
        try
        {
            Vibration.Vibrate(TimeSpan.FromMilliseconds(80));
        }
        catch
        {
            // algunos dispositivos no soportan vibración
        }
    }

    async Task VibrarDoble()
    {
        Vibration.Vibrate(50);
        await Task.Delay(100);
        Vibration.Vibrate(50);
    }


}