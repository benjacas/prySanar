using System;
using System.Collections.Generic;
using System.Text;

namespace pryPrueba.Clases.Burbuja.Servicios
{
    public class clsSesionTimer
    {
        IDispatcherTimer _timer;
        TaskCompletionSource<bool> _tcs;

        public event Action<TimeSpan> Tick;

        public Task Iniciar(TimeSpan duracion)
        {
            _tcs = new TaskCompletionSource<bool>();

            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);

            var restante = duracion;

            _timer.Tick += (s, e) =>
            {
                restante -= TimeSpan.FromSeconds(1);
                Tick?.Invoke(restante);

                if (restante <= TimeSpan.Zero)
                {
                    _timer.Stop();
                    _tcs.TrySetResult(true);
                }
            };

            _timer.Start();
            return _tcs.Task;
        }

        public void Detener() => _timer?.Stop();
    }

}
