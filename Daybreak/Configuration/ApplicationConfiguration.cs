using Daybreak.Properties;

namespace Daybreak.Configuration
{
    public class ApplicationConfigurationValidator
    {
        private readonly Settings _settings;

        internal ApplicationConfigurationValidator(Settings settings)
        {
            _settings = settings;
        }

        public bool IsValid()
        {
            return
                !string.IsNullOrWhiteSpace(_settings.SunriseSunsetApiUrl) &&
                !string.IsNullOrWhiteSpace(_settings.YahooapisUrl);
        }
    }
}
