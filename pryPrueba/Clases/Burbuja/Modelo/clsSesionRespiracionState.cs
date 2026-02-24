using System;
using System.Collections.Generic;
using System.Text;

namespace pryPrueba.Clases.Burbuja.Modelo
{
    public class clsSesionRespiracionState
    {
        public bool Animado {  get; set; }
        public bool Pausado { get; set; }
        public string FaseActual { get; set; } = "";
        public double EscalaActual { get; set; } = 1.0;

        public TimeSpan TiempoReestante { get; set; } = TimeSpan.FromMinutes(5);

    }
}
