namespace pryPrueba;
using Microsoft.Maui.Essentials;
using pryPrueba.Clases.Burbuja.Animacion;
using pryPrueba.Clases.Burbuja.Modelo;
using pryPrueba.Clases.Burbuja.Servicios;
using pryPrueba.Pantallas;

public partial class frmBurbuja : ContentPage
{
 
        clsSesionRespiracionState _state;
        SesionRespiracionService _sesion;
        AnimadorCirculo _animador;

        public frmBurbuja(string dificultad)
        {
            InitializeComponent();

            _state = new clsSesionRespiracionState();
            _sesion = new SesionRespiracionService(_state);
            _animador = new AnimadorCirculo(Circulo, _state);

            _sesion.EstadoCambiado += e => lblEstado.Text = e;
            _sesion.ContadorActualizado += v => lblTiempo.Text = v.ToString();
        }

    void BtnPausa_Clicked(object sender, EventArgs e)
    {
        if (_state.Pausado)
            _sesion.Reanudar();
        else
            _sesion.Pausar();

        BtnPausa.Text = _state.Pausado ? "Reanudar" : "Pausar";
    }

}