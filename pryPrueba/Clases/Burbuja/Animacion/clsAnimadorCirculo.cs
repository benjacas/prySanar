using pryPrueba.Clases.Burbuja.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace pryPrueba.Clases.Burbuja.Animacion
{
    public class AnimadorCirculo
    {
        readonly VisualElement _elemento;
        readonly clsSesionRespiracionState _state;

        public AnimadorCirculo(VisualElement elemento, clsSesionRespiracionState state)
        {
            _elemento = elemento;
            _state = state;
        }

        public void Animar(double desde, double hasta, uint duracion)
        {
            var anim = new Animation(v =>
            {
                _state.EscalaActual = v;
                _elemento.Scale = v;
            }, desde, hasta);

            anim.Commit(
                _elemento,
                "RespiracionAnim",
                length: duracion,
                easing: Easing.SinInOut
            );
        }

        public void Detener()
        {
            _elemento.AbortAnimation("RespiracionAnim");
        }
    }

}
