namespace pryPrueba
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Card1_Tapped(object sender, EventArgs e)
        {

            var card = (Frame)sender;

            // Animación de agrandado (zoom)
            await card.ScaleTo(1.05, 100, Easing.CubicIn);   // 5% más grande
            await card.ScaleTo(1.0, 100, Easing.CubicOut);   // vuelve al tamaño normal

            // Acción después de la animación
            await Navigation.PushAsync(new frmListaEjercicios());
        }

        private async void Card2_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new frmListaEjercicios());
        }



    }
}
