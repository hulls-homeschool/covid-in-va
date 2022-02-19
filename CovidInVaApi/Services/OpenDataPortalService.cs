using Microsoft.Extensions.Options;

namespace CovidInVaApi.Services
{
    public class OpenDataPortalService : BackgroundService
    {
        private const int MillisecondsDelay = 60 * 60 * 1000; // One hour

        private readonly string _appToken;
        private readonly string _url;

        public OpenDataPortalService(IOptions<OpenDataPortalSettings> settings)
        {
            _appToken = settings.Value.AppToken;
            _url = settings.Value.Url;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startDate = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local);
            while (!stoppingToken.IsCancellationRequested)
            {
                var url = $"{_url}?report_date={startDate:s}";

                var handler = new HttpClientHandler();
                using var client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add("X-App-Token", _appToken);
                var response = await client.GetAsync(url, stoppingToken);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync(stoppingToken);

                    startDate = startDate.AddDays(1);
                }

                await Task.Delay(MillisecondsDelay, stoppingToken);
            }
        }
    }
}
