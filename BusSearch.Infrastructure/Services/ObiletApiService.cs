using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BusSearch.Application.Interfaces;
using BusSearch.Application.Constants;
using BusSearch.Application.Models.Dtos;
using BusSearch.Application.Exceptions;
using BusSearch.Infrastructure.Configurations;
using BusSearch.Infrastructure.Models.ObiletApi.Location;
using BusSearch.Infrastructure.Models.ObiletApi.Journey;
using BusSearch.Infrastructure.Models.ObiletApi.Sessions;
using BusSearch.Infrastructure.Models.ObiletApi;
using System.Net.Http.Json;
using BusSearch.Infrastructure.Mappers;
using System.Text.Json;

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

        public ObiletApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<ObiletApiService> logger, IOptions<ClientDefaults> clientDefaults)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _clientDefaults = clientDefaults.Value;
        }

        public async Task<List<BusLocationDto>> GetAllBusLocationsAsync()
        {
            await EnsureSessionAsync();
            var request = CreateDeviceRequest(new BusLocationRequest { Data = null });
            var response = await SendRequestAsync<ObiletResponse<List<BusLocationResponse>>>(BusLocationsUrl, request);
            
            if (response?.Data is null)
            {
                throw new ObiletApiException("Otobüs lokasyonları alınamadı. Lütfen tekrar deneyiniz.");
            }

            return LocationMapper.ToDtoList(response.Data);
        }

        public async Task<List<BusLocationDto>> SearchBusLocationsAsync(string keyword)
        {
            await EnsureSessionAsync();
            var request = CreateDeviceRequest(new BusLocationRequest { Data = keyword });
            var response = await SendRequestAsync<ObiletResponse<List<BusLocationResponse>>>(BusLocationsUrl, request);

            if (response?.Data is null)
            {
                throw new ObiletApiException("Lokasyon araması sonucunda geçerli veri alınamadı.");
            }

            return LocationMapper.ToDtoList(response.Data);
        }

        public async Task<List<JourneyDto>> GetJourneysAsync(int originId, int destinationId, string departureDate)
        {
            await EnsureSessionAsync();

            var request = CreateDeviceRequest(new JourneyRequest
            {
                Data = new JourneyData
                {
                    OriginId = originId,
                    DestinationId = destinationId,
                    DepartureDate = departureDate
                }
            });

            var response = await SendRequestAsync<ObiletResponse<List<JourneyResponse>>>(JourneyUrl, request);

            if (response?.Data is null)
            {
                throw new ObiletApiException("Sunucudan sefer bilgileri alınamadı.");
            }

            if (!response.Data.Any())
            {
                _logger.LogWarning("Sefer bulunamadı. OriginId: {OriginId}, DestinationId: {DestinationId}", originId, destinationId);
            }

            return JourneyMapper.ToDtoList(response.Data);
        }

        public async Task<SessionResponse> CreateSessionAsync()
        {
            var request = new SessionRequest
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

            var response = await SendRequestAsync<ObiletResponse<SessionResponse>>(GetSessionUrl, request);

            if (response?.Data is null)
            {
                throw new ObiletApiException("Sunucudan geçerli oturum verisi alınamadı.");
            }

            SessionId = response.Data.SessionId;
            DeviceId = response.Data.DeviceId;

            return response.Data;
        }

        private async Task EnsureSessionAsync()
        {
            if (!string.IsNullOrEmpty(SessionId) && !string.IsNullOrEmpty(DeviceId))
                return;

            await CreateSessionAsync();
        }

        private async Task<T?> SendRequestAsync<T>(string url, object requestBody)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("{Url} çağrısı başarısız. StatusCode: {StatusCode}, İçerik: {Content}", url, response.StatusCode, content);
                    throw new ObiletApiException(
                        message: $"Obilet API çağrısı başarısız. StatusCode: {response.StatusCode}",
                        statusCode: response.StatusCode,
                        responseContent: content
                    );
                }

                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null)
                {
                    _logger.LogError("JSON yanıtı deserialize edilemedi (null döndü). Url: {Url}, İçerik: {Content}", url, content);
                    throw new ObiletApiException("Sunucudan geçerli yanıt alınamadı.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parse hatası. Url: {Url}", url);
                throw new ObiletApiException("Obilet API yanıtı çözümlenemedi.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Url} çağrısı sırasında beklenmeyen hata oluştu.", url);
                throw new ObiletApiException("Obilet API çağrısı sırasında bir hata oluştu.", ex);
            }
        }



        private T CreateDeviceRequest<T>(T request) where T : IDeviceRequest
        {
            request.DeviceSession = new DeviceSession
            {
                SessionId = SessionId ?? throw new InvalidOperationException("SessionId boş."),
                DeviceId = DeviceId ?? throw new InvalidOperationException("DeviceId boş.")
            };
            request.Date = DateTime.Now;
            request.Language = Languages.DefaultLanguage;
            return request;
        }
    }
}
