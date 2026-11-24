namespace ControlGastos
{
    public partial class ResumenPage : ContentPage
    {
        public ResumenPage()
        {
            InitializeComponent();
        }

        // Se recalcula el resumen cada vez que se abre la pestaña
        private async void OnPageAppearing(object sender, EventArgs e)
        {
            var gastos = await App.Database.GetGastosAsync();

            var resumen = gastos
                .GroupBy(g => g.Categoria)
                .Select(grupo => new ResumenCategoria
                {
                    Categoria = grupo.Key,
                    Cantidad = grupo.Count(),
                    Total = grupo.Sum(g => g.Monto)
                })
                .OrderByDescending(r => r.Total)
                .ToList();

            ResumenListView.ItemsSource = resumen;
            TotalGeneralLabel.Text = $"Total general: ${gastos.Sum(g => g.Monto):F2}";
        }
    }
}
