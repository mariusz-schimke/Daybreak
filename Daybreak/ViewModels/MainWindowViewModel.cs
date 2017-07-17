using Daybreak.Commands;
using System;
using System.Windows.Input;
using PropertyChanged;

namespace Daybreak.ViewModels
{
    [AddINotifyPropertyChangedInterface()]
    public class MainWindowViewModel
    {
        public string Location { get; set; } = "Bielsko-Biała";
        public DateTime? Date { get; set; } = DateTime.Now;

        public string Sunrise { get; set; }
        public string Sunset { get; set; } 
        public string DayLength { get; set; }

        public bool QueryEnabled { get; set; } = true;
        public ICommand QueryCommand { get; set; }

        public MainWindowViewModel()
        {
            QueryCommand = new QueryCommand(this);
        }

        public void SetQueryResults(DateTime sunrise, DateTime sunset, TimeSpan dayLength)
        {
            Sunrise = sunrise.ToLocalTime().ToString("HH\\:mm\\:ss");
            Sunset = sunset.ToLocalTime().ToString("HH\\:mm\\:ss");
            DayLength = dayLength.ToString("h\" godzin \"m\" minut \"s\" sekund\"");
        }

        public void ResetQueryResults()
        {
            Sunrise = Sunset = DayLength = string.Empty;
        }
    }
}
