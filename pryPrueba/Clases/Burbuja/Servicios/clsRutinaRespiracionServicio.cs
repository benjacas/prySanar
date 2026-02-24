using pryPrueba.Clases.Burbuja.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace pryPrueba.Clases.Burbuja.Servicios
{
    public class RutinaRespiracion
    {
        public uint DuracionAnimacion { get; set; }
        public int Segundos { get; set; }
        public int Repeticiones { get; set; }
    }


    public static class RutinaRespiracionProvider
    {
        public static List<RutinaRespiracion> Obtener(string dificultad)
        {
            return dificultad switch
            {
                "Facil" => new()
            {
                new RutinaRespiracion { DuracionAnimacion = 2000, Segundos = 2, Repeticiones = 6 },
                new RutinaRespiracion { DuracionAnimacion = 5000, Segundos = 5, Repeticiones = 1 }
            },

                "Medio" => new()
            {
                new RutinaRespiracion { DuracionAnimacion = 3000, Segundos = 3, Repeticiones = 6 },
                new RutinaRespiracion { DuracionAnimacion = 10000, Segundos = 10, Repeticiones = 1 }
            },

                "Dificil" => new()
            {
                new RutinaRespiracion { DuracionAnimacion = 2000, Segundos = 2, Repeticiones = 1 },
                new RutinaRespiracion { DuracionAnimacion = 15000, Segundos = 15, Repeticiones = 1 }
            },

                _ => new()
            };
        }
    }

}
