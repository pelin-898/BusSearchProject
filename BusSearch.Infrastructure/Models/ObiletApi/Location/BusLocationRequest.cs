﻿using BusSearch.Application.Constants;
using System.Text.Json.Serialization;

namespace BusSearch.Infrastructure.Models.ObiletApi.Location
{
    public class BusLocationRequest : IDeviceRequest
    {
        [JsonPropertyName("data")]
        public string? Data { get; set; } = null;

        [JsonPropertyName("device-session")]
        public DeviceSession DeviceSession { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonPropertyName("language")]
        public string Language { get; set; } = Languages.DefaultLanguage;
    }

    public class DeviceSession
    {
        [JsonPropertyName("session-id")]
        public string SessionId { get; set; }

        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }
    }
}
