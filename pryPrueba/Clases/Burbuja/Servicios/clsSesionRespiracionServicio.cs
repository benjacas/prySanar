//using Kotlin.Contracts;
using pryPrueba.Clases.Burbuja.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace pryPrueba.Clases.Burbuja.Servicios
{
    public class SesionRespiracionService
    {
        readonly clsSesionRespiracionState _state;
        readonly clsSesionTimer _timer;

        public event Action<string> EstadoCambiado;
        public event Action<int> ContadorActualizado;

        public SesionRespiracionService(clsSesionRespiracionState state)
        {
            _state = state;
            _timer = new clsSesionTimer();
        }

        public async Task IniciarSesion(string dificultad, Func<uint, int, Task> respirar)
        {
            _state.Animado = true;

            for (int ciclo = 0; ciclo < 3; ciclo++)
            {
                var rutinas = RutinaRespiracionProvider.Obtener(dificultad);

                foreach (var r in rutinas)
                {
                    for (int i = 0; i < r.Repeticiones && _state.Animado; i++)
                        await respirar(r.DuracionAnimacion, r.Segundos);
                }

                if (ciclo < 2)
                    await Descanso();
            }

            _state.Animado = false;
        }

        async Task Descanso()
        {
            EstadoCambiado?.Invoke("Descanso");

            for (int i = 10; i > 0; i--)
            {
                ContadorActualizado?.Invoke(i);
                await Task.Delay(1000);
            }
        }
    }

}
