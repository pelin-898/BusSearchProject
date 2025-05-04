using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BusSearch.Application.Interfaces;
using BusSearch.Application.Constants;
using BusSearch.Application.Models.Dtos;
using BusSearch.Infrastructure.Configurations;
using BusSearch.Infrastructure.Models.ObiletApi.Location;
using BusSearch.Infrastructure.Models.ObiletApi.Journey;
using BusSearch.Infrastructure.Models.ObiletApi.Sessions;
using BusSearch.Infrastructure.Models.ObiletApi;

namespace BusSearch.Infrastructure.Services
{
    public class ObiletApiService : IObiletApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ObiletApiService> _logger;
        private readonly ClientDefaults _clientDefaults;

        private const string GetSessionUrl = "api/client/getsession";
        private const string JourneyUrl = "api/journey/getbusjourneys";
        private const string BusLocationsUrl = "api/location/getbuslocations";

        private string? SessionId
        {
            get => _httpContextAccessor.HttpContext?.Session.GetString("SessionId");
            set => _httpContextAccessor.HttpContext?.Session.SetString("SessionId", value!);
        }

        private string? DeviceId
        {
            get => _httpContextAccessor.HttpContext?.Session.GetString("DeviceId");
            set => _httpContextAccessor.HttpContext?.Session.SetString("DeviceId", value!);
        }

        public ObiletApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ObiletApiService> logger,
            IOptions<ClientDefaults> clientDefaults)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _clientDefaults = clientDefaults.Value;
        }

        public async Task EnsureSessionAsync()
        {
            if (!string.IsNullOrEmpty(SessionId) && !string.IsNullOrEmpty(DeviceId))
                return;

            _logger.LogInformation("Yeni session oluşturuluyor...");

            var request = CreateSessionRequest();
            var response = await SendRequestAsync<SessionResponse>(GetSessionUrl, request);

            if (response?.Data is null)
            {
                _logger.LogError("Session oluşturulamadı. Sunucudan geçerli yanıt alınamadı.");
                throw new Exception("Session oluşturulamadı.");
            }

            SessionId = response.Data.SessionId;
            DeviceId = response.Data.DeviceId;

            _logger.LogInformation("Session oluşturuldu. SessionId: {SessionId}, DeviceId: {DeviceId}", SessionId, DeviceId);
        }

        public async Task<List<JourneyItemDto>> GetJourneysAsync(int originId, int destinationId, string departureDate)
        {
            await EnsureSessionAsync();

            _logger.LogInformation("Seferler alınıyor. OriginId: {OriginId}, DestinationId: {DestinationId}, Date: {Date}",
                originId, destinationId, departureDate);

            var request = CreateDeviceRequest(new JourneyRequest
            {
                Data = new JourneyData
                {
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate
                }
            });

            var response = await SendRequestAsync<JourneyResponse>(JourneyUrl, request);

            return response?.Data?.Select(j => new JourneyItemDto
            {
                OriginLocation = j.OriginLocation,
                DestinationLocation = j.DestinationLocation,
                OriginTerminal = j.Journey.Origin,
                DestinationTerminal = j.Journey.Destination,
                Departure = j.Journey.Departure,
                Arrival = j.Journey.Arrival,
                Price = j.Journey.InternetPrice
            }).OrderBy(j => j.Departure).ToList() ?? new();
        }

        public async Task<List<BusLocationDto>> GetAllBusLocationsAsync()
        {
            await EnsureSessionAsync();

            _logger.LogInformation("Tüm otobüs lokasyonları alınıyor...");

            var request = CreateDeviceRequest(new BusLocationRequest { Data = null });

            var response = await SendRequestAsync<BusLocationResponse>(BusLocationsUrl, request);

            return response?.Data?.Select(l => new BusLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                CityName = l.CityName,
                Rank = l.Rank
            }).ToList() ?? new();
        }

        public async Task<List<BusLocationDto>> SearchBusLocationsAsync(string keyword)
        {
            await EnsureSessionAsync();

            _logger.LogInformation("Lokasyon aranıyor. Anahtar kelime: {Keyword}", keyword);

            var request = CreateDeviceRequest(new BusLocationRequest { Data = keyword });

            var response = await SendRequestAsync<BusLocationResponse>(BusLocationsUrl, request);

            return response?.Data?.Select(l => new BusLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                CityName = l.CityName,
                Rank = l.Rank
            }).ToList() ?? new();
        }

        private async Task<T?> SendRequestAsync<T>(string url, object requestBody)
        {
            try
            {
                _logger.LogDebug("POST isteği gönderiliyor -> {Url}", url);

                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("{Url} çağrısı başarısız. StatusCode: {StatusCode}, İçerik: {Content}", url, response.StatusCode, content);
                    return default;
                }

                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parse hatası. Url: {Url}", url);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İstek gönderilirken beklenmedik bir hata oluştu. Url: {Url}", url);
                return default;
            }
        }

        private T CreateDeviceRequest<T>(T request) where T : IDeviceRequest
        {
            request.DeviceSession = new DeviceSession
            {
                SessionId = SessionId!,
                DeviceId = DeviceId!
            };
            request.Date = DateTime.Now;
            request.Language = Languages.DefaultLanguage;

            return request;
        }

        private SessionRequest CreateSessionRequest()
        {
            return new SessionRequest
            {
                Type = 1,
                Connection = new Connection
                {
                    IpAddress = _clientDefaults.IpAddress,
                    Port = _clientDefaults.Port
                },
                Browser = new Browser
                {
                    Name = _clientDefaults.BrowserName,
                    Version = _clientDefaults.BrowserVersion
                }
            };
        }
    }
}
