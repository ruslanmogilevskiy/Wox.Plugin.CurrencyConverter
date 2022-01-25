using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Wox.Infrastructure.Storage;

namespace Wox.Plugin.CurrencyConverter.PluginSettings
{
    public partial class SettingsControl : UserControl
    {
        PluginJsonStorage<CurrencyConverterSettings> storage;
        CurrencyConverterSettings settings;

        public SettingsControl()
        {
            InitializeComponent();

            storage = new PluginJsonStorage<CurrencyConverterSettings>();
            settings = storage.Load();

            apiKeyText.Text = settings.ApiKey;
            favoriteCurrencies.Text = settings.FavoriteCurrencies;
        }

        void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            e.Handled = true;
        }

        void OnApplySettings(object sender, RoutedEventArgs e)
        {
            settings.ApiKey = apiKeyText.Text;
            settings.FavoriteCurrencies = favoriteCurrencies.Text;
            storage.Save();
        }
    }
}
