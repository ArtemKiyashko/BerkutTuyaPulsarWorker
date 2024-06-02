﻿using System.Text.Json.Serialization;

namespace BerkutTuyaPulsarWorker.Models;

public class DeviceStatus
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("value")]
    public dynamic Value { get; set; }

    [JsonPropertyName("t")]
    public long Timestamp { get; set; }
}
