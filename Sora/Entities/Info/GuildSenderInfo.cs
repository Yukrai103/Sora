﻿using Newtonsoft.Json;

namespace Sora.Entities.Info;

/// <summary>
/// 频道消息发送者
/// </summary>
public struct GuildSenderInfo
{
    /// <summary>
    /// 发送者昵称
    /// </summary>
    [JsonProperty(PropertyName = "nickname")]
    public string Nick { get; internal set; }

    /// <summary>
    /// 发送者ID
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public long UserId { get; internal set;}
}