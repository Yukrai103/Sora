using Newtonsoft.Json;
using Sora.Converter;

namespace Sora.Entities.MessageSegment.Segment
{
    /// <summary>
    /// 回复
    /// </summary>
    public class ReplySegment : BaseSegment
    {
        #region 属性

        /// <summary>
        /// 消息ID
        /// </summary>
        [JsonConverter(typeof(StringConverter))]
        [JsonProperty(PropertyName = "id")]
        public int Target { get; internal set; }

        #endregion
    }
}