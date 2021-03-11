using System;
using System.Threading.Tasks;
using Sora.Exceptions;
using Sora.OnebotInterface;
using Sora.OnebotModel;

namespace Sora.Net
{
    /// <summary>
    /// Sora 连接服务
    /// </summary>
    public interface ISoraService
    {
        /// <summary>
        /// 事件接口
        /// </summary>
        public EventInterface Event { get; }

        /// <summary>
        /// 服务器连接管理器
        /// </summary>
        public ConnectionManager ConnManager { get; }

        /// <summary>
        /// 启动 Sora 服务
        /// </summary>
        /// <exception cref="SoraServerIsRuningException">已有服务器在运行</exception>
        public ValueTask StartService()
            => this switch
            {
                SoraWebsocketServer s1 => s1.StartServer(),
                SoraWebsocketClient s2 => s2.StartClient(),
                _ => throw new ArgumentException("接收到了不认识的 Sora 服务对象。")
            };

        /// <summary>
        /// Sora 实例工厂
        /// </summary>
        /// <param name="config">服务器配置</param>
        /// <param name="crashAction">发生未处理异常时的回调</param>
        /// <returns></returns>
        static ISoraService SoraServiceFactory(ISoraConfig config, Action<Exception> crashAction = null)
            => config switch
            {
                ClientConfig s1 => new SoraWebsocketClient(s1, crashAction),
                ServerConfig s2 => new SoraWebsocketServer(s2, crashAction),
                _ => throw new ArgumentException("接收到了不认识的 Sora 配置对象。")
            };
    }
}