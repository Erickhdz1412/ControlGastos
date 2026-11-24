namespace ControlGastos
{
    public partial class MainPage : ContentPage
    {
        private List<Gasto> _gastos = new();

        private List<string> _categorias = new() { "Comida", "Transporte", "Escuela", "Entretenimiento", "Otros" };

        public MainPage()
        {
            InitializeComponent();
            CategoriaPicker.ItemsSource = _categorias;
            CategoriaPicker.SelectedIndex = 0;
            FechaPicker.Date = DateTime.Today;
            LoadGastos();
        }

        private async void LoadGastos()
        {
            _gastos = await App.Database.GetGastosAsync();
            MostrarGastos();
        }

        // Muestra la lista (aplicando el texto de busqueda) y actualiza el total
        private void MostrarGastos()
        {
            string texto = BuscarBar.Text ?? string.Empty;

            var lista = _gastos
                .Where(g => g.Descripcion.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(g => g.Fecha)
                .ToList();

            GastosListView.ItemsSource = lista;
            TotalLabel.Text = $"Total: ${lista.Sum(g => g.Monto):F2}";
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescripcionEntry.Text) ||
                !double.TryParse(MontoEntry.Text, out double monto) ||
                monto <= 0 ||
                CategoriaPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Descripción, monto y categoría válidos son requeridos", "OK");
                return;
            }

            var nuevoGasto = new Gasto
            {
                Descripcion = DescripcionEntry.Text,
                Monto = monto,
                Categoria = CategoriaPicker.SelectedItem.ToString()!,
                Fecha = FechaPicker.Date ?? DateTime.Today
            };

            await App.Database.AddGastoAsync(nuevoGasto);
            LoadGastos();

            DescripcionEntry.Text = string.Empty;
            MontoEntry.Text = string.Empty;
        }

        private void OnBuscarTextChanged(object sender, TextChangedEventArgs e)
        {
            MostrarGastos();
        }

        private void OnFechaSelected(object sender, DateChangedEventArgs e)
        {
            // Evita registrar gastos con fecha en el futuro
            if (e.NewDate.HasValue && e.NewDate.Value > DateTime.Today)
            {
                FechaPicker.Date = DateTime.Today;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Gasto gasto)
            {
                bool confirm = await DisplayAlert("Confirmar", $"¿Eliminar {gasto.Descripcion}?", "Sí", "No");
                if (confirm)
                {
                    await App.Database.DeleteGastoAsync(gasto);
                    LoadGastos();
                }
            }
        }

        // Al tocar un gasto se edita su monto
        private async void OnGastoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Gasto gasto)
            {
                string nuevoMonto = await DisplayPromptAsync("Editar", "Nuevo monto:",
                    keyboard: Keyboard.Numeric, initialValue: gasto.Monto.ToString());

                if (double.TryParse(nuevoMonto, out double monto) && monto > 0 && monto != gasto.Monto)
                {
                    gasto.Monto = monto;
                    await App.Database.UpdateGastoAsync(gasto);
                    LoadGastos();
                }

                // Deseleccionar
                GastosListView.SelectedItem = null;
            }
        }
    }
}
