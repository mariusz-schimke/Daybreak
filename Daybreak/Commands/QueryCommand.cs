using Autofac;
using Daybreak.Properties;
using Daybreak.ViewModels;
using Services.SunriseSunsetWithGeocoding;
using System;
using System.Windows;
using System.Windows.Input;

namespace Daybreak.Commands
{
    public class QueryCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public QueryCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.QueryEnabled && 
                !string.IsNullOrWhiteSpace(_viewModel.Location) &&
                _viewModel.Date.HasValue;
        }

        public async void Execute(object parameter)
        {
            var service = ((App)Application.Current).Container.Resolve<ISunriseSunsetWithGeocodingService>();

            _viewModel.ResetQueryResults();

            _viewModel.QueryEnabled = false;
            var serviceResult = await service.GetSunriseSunsetAsync(_viewModel.Location, _viewModel.Date.Value);
            _viewModel.QueryEnabled = true;
            
            if (serviceResult != null)
            {
                _viewModel.SetQueryResults(
                    serviceResult.Sunrise, 
                    serviceResult.Sunset, 
                    serviceResult.DayLength);

                if (serviceResult.MatchingQueryLocations > 1)
                {
                    MessageBox.Show(App.Current.MainWindow, "Dla podanej lokalizacji znaleziono wiele dopasowań. Spróbuj uściślić dane.");
                }
            }
            else
            {
                MessageBox.Show(App.Current.MainWindow, "Pobranie informacji dla podanej lokalizacji było niemożliwe.");
            }
        }
    }
}
