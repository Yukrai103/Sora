using System;
using Sora.EventArgs.OnebotEvent.NoticeEvent;
using Sora.Module;

namespace Sora.EventArgs.SoraEvent
{
    public sealed class GroupRecallEventArgs : BaseSoraEventArgs
    {
        #region 属性
        /// <summary>
        /// 消息发送者
        /// </summary>
        public User MessageSender { get; private set; }

        /// <summary>
        /// 撤回执行者
        /// </summary>
        public User Operator { get; private set; }

        /// <summary>
        /// 消息源群
        /// </summary>
        public Group SourceGroup { get; private set; }

        /// <summary>
        /// 被撤消息ID
        /// </summary>
        public int MessageId { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connectionGuid">服务器链接标识</param>
        /// <param name="eventName">事件名</param>
        /// <param name="groupRecall">群聊撤回事件参数</param>
        internal GroupRecallEventArgs(Guid connectionGuid, string eventName, ApiGroupRecallEventArgs groupRecall) :
            base(connectionGuid, eventName, groupRecall.SelfID, groupRecall.Time)
        {
            this.MessageSender = new User(connectionGuid, groupRecall.UserId);
            //执行者和发送者可能是同一人
            this.Operator = groupRecall.UserId == groupRecall.OperatorId
                ? this.MessageSender
                : new User(connectionGuid, groupRecall.OperatorId);
            this.SourceGroup = new Group(connectionGuid, groupRecall.GroupId);
            this.MessageId   = groupRecall.MessageId;
        }
        #endregion
    }
}