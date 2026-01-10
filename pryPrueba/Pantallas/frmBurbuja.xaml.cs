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
    TaskCompletionSource<bool> _tcsTemporizador;

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


    async Task IniciarSesion()
    {
        for (int ciclo = 0; ciclo < 3; ciclo++)
        {
            MarcarCiclo(ciclo);
            await IniciarBloqueRespiracion();

            // descanso solo si no es el último ciclo
            if (ciclo < 2)
                await IniciarDescanso();
        }

        FinalizarSesion();
    }

    async Task IniciarBloqueRespiracion()
    {
        _tiempoRestante = TimeSpan.FromMinutes(5);
        _animado = true;

        var respiracionTask = RespirarLoop();
        await Temporizador();     // espera 5 minutos
        _animado = false;         // corta respiración
        await respiracionTask;    // espera a que termine limpio
    }
    async Task RespirarLoop()
    {
        while (_animado)
        {
            await RespirarSegunDificultad();
        }
    }

    async Task RespirarSegunDificultad()
    {
        while (_animado)
        {
            switch (_dificultad)
            {
                case "Facil": 
                    for (int i = 0; i < 6; i++)
                    {
                        await Respirar(2000,2);
                    }

                    for (int i = 0; i < 1; i++)
                    {
                        await Respirar(5000,5);
                    }
                break;

                case "Medio":
                    for (int i = 0; i < 6; i++)
                    {
                        await Respirar(3000, 3);
                    }

                    for (int i = 0; i < 1; i++)
                    {
                        await Respirar(10000, 10);
                    }
                    break;

                case "Dificil":
                    for (int i = 0; i < 1; i++)
                    {
                        await Respirar(2000, 2);
                    }

                    for (int i = 0; i < 1; i++)
                    {
                        await Respirar(15000, 15);
                    }
                    break;
            }
        }
    }



    async Task Respirar(uint duracionAnimacion, int contarRespiracion) //Esto despues se puede modificar los segundos que se exhala e inhala para cambiar de dificultad, por ahora probemos si esto funciona
	{

                //inhalar
                lblEstado.Text = "Inhalar";
                AnimarCirculo(_escalaActual, 1.4, duracionAnimacion);
                _faseActual = "Inhalar";
                await ContarRespiracion(contarRespiracion);
                Circulo.BackgroundColor = Colors.Pink;
                //VibrarSuave();

                if (!_animado) return;

                //Sostener
                lblEstado.Text = "Sostener";
                _faseActual = "Sostener";
                await ContarRespiracion(contarRespiracion);
                AnimarCirculo(_escalaActual, 1.4, duracionAnimacion);//probando
                Circulo.BackgroundColor = Colors.Green;
                //await VibrarDoble();

                if (!_animado) return;

                //Exhalar
                lblEstado.Text = "Exhalar";
                _faseActual = "Exhalar";
                AnimarCirculo(_escalaActual, 1.0, duracionAnimacion);
                await ContarRespiracion(contarRespiracion);
                Circulo.BackgroundColor = Colors.Purple;


                if (!_animado) return;
                //VibrarSuave();
  
    }

    async Task IniciarDescanso()
    {
        lblEstado.Text = "Descanso";
        _faseActual = "Descanso";

        Circulo.AbortAnimation("RespiracionAnim");
        Circulo.BackgroundColor = Colors.LightBlue;

        await ContarDescanso(10);
    }

    async Task ContarDescanso(int segundos)
    {
        for (int i = segundos; i > 0; i--)
        {
            lblTiempo.Text = i.ToString();
            await Task.Delay(1000);
        }
    }


    Task Temporizador()
    {
        _tcsTemporizador = new TaskCompletionSource<bool>();

        _timerSesion = Dispatcher.CreateTimer();
        _timerSesion.Interval = TimeSpan.FromSeconds(1);

        _timerSesion.Tick += (s, e) =>
        {
            if (_pausado) return;

            _tiempoRestante -= TimeSpan.FromSeconds(1);
            lblTemporizador.Text = _tiempoRestante.ToString(@"mm\:ss");

            if (_tiempoRestante <= TimeSpan.Zero)
            {
                _timerSesion.Stop();
                _animado = false;
                _tcsTemporizador.TrySetResult(true);
            }
        };

        _timerSesion.Start();
        return _tcsTemporizador.Task;
    }


    void FinalizarSesion()
    {
        lblEstado.Text = "Sesión finalizada";
        Circulo.AbortAnimation("RespiracionAnim");
        _timerSesion?.Stop();
    }


    protected override async void OnAppearing()
	{
		base.OnAppearing();

        _animado = true;
        await IniciarSesion();
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

    void MarcarCiclo(int cicloActual)
    {
        var cards = new[]
        {
        PrimeralCard,
        SegundaCard,
        TerceraCard

        };

        var colores = new[]
        {
        Colors.LightGreen,
        Colors.Orange,
        Colors.LightCoral
        };

        for (int i = 0; i < cards.Length; i++)
        {
            if (i == cicloActual)
            {
                cards[i].BackgroundColor = colores[i];
                cards[i].StrokeThickness = 2;
            }
            else
            {
                cards[i].BackgroundColor = Colors.Transparent;
                cards[i].StrokeThickness = 1;
            }
        }
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