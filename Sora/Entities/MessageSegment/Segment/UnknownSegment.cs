﻿using Newtonsoft.Json.Linq;

namespace Sora.Entities.MessageSegment.Segment
{
    /// <summary>
    /// 未知消息段
    /// </summary>
    public class UnknownSegment : BaseSegment
    {
        #region 属性

        /// <summary>
        /// 纯文本内容
        /// </summary>
        public JObject Content { get; internal set; }

        #endregion
    }
}