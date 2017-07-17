using Autofac;
using Daybreak.Configuration;
using Daybreak.Properties;
using Services.Geocoding;
using Services.SunriseSunset;
using Services.SunriseSunsetWithGeocoding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IContainer Container { get; set; }

        private void Application_Startup(object sender, EventArgs e)
        {
            var configValidator = new ApplicationConfigurationValidator(Settings.Default);
            if (!configValidator.IsValid())
            {
                MessageBox.Show(App.Current.MainWindow, "Konfiguracja programu jest nieprawidłowa. Adresy URL API nie zostały ustawione. Program zostanie zamknięty.");
                Shutdown();
            }

            var builder = new ContainerBuilder();
            builder.RegisterType<GeocodingService>().As<IGeocodingService>()
                .WithParameter(new TypedParameter(typeof(string), Settings.Default.YahooapisUrl));
            builder.RegisterType<SunriseSunsetService>().As<ISunriseSunsetService>()
                .WithParameter(new TypedParameter(typeof(string), Settings.Default.SunriseSunsetApiUrl));
            builder.RegisterType<SunriseSunsetWithGeocodingService>().As<ISunriseSunsetWithGeocodingService>();

            Container = builder.Build();
        }
    }
}
