using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sora.Entities;
using Sora.OnebotModel.ApiParams;
using YukariToolBox.Extensions;
using YukariToolBox.FormatLog;

namespace Sora.Net
{
    /// <summary>
    /// 用于管理和发送API请求
    /// </summary>
    internal static class ReactiveApiManager
    {
        #region 通信

        /// <summary>
        /// 获取到API响应
        /// </summary>
        /// <param name="echo">标识符</param>
        /// <param name="response">响应json</param>
        internal static void GetResponse(Guid echo, JObject response)
        {
            Log.Debug("Sora|ReactiveApiManager", $"Get api response {response.ToString(Formatting.None)}");
            StaticVariable.ApiSubject.OnNext(new Tuple<Guid, JObject>(echo, response));
        }

        /// <summary>
        /// 向API客户端发送请求数据
        /// </summary>
        /// <param name="apiRequest">请求信息</param>
        /// <param name="connectionGuid">服务器连接标识符</param>
        /// <param name="timeout">覆盖原有超时,在不为空时有效</param>
        /// <returns>API返回</returns>
        internal static async ValueTask<JObject> SendApiRequest(ApiRequest apiRequest, Guid connectionGuid,
                                                                TimeSpan? timeout = null)
        {
            TimeSpan currentTimeout;
            if (timeout is null)
            {
                if (!ConnectionManager.GetApiTimeout(connectionGuid, out currentTimeout))
                {
                    Log.Error("Sora|ReactiveApiManager", "Cannot get api timout");
                    currentTimeout = TimeSpan.FromSeconds(5);
                }
            }
            else
            {
                Log.Debug("Sora|ReactiveApiManager", $"timeout covered to {timeout.Value.TotalMilliseconds} ms");
                currentTimeout = (TimeSpan) timeout;
            }

            //向客户端发送请求数据
            var apiTask = StaticVariable.ApiSubject
                                        .Where(request => request.Item1 == apiRequest.Echo)
                                        .Select(request => request.Item2)
                                        .Take(1)
                                        .Timeout(currentTimeout)
                                        .Catch(Observable.Return(new JObject()))
                                        .ToTask()
                                        .RunCatch(e =>
                                                  {
                                                      Log.Error("Sora|ReactiveApiManager",
                                                                $"ApiSubject Error {Log.ErrorLogBuilder(e)}");
                                                      return new JObject();
                                                  });
            if (!ConnectionManager.SendMessage(connectionGuid, JsonConvert.SerializeObject(apiRequest, Formatting.None))
            ) return null;
            //等待客户端返回调用结果
            var response = await apiTask.ConfigureAwait(false);
            if (response != null && response.Count != 0) return response;
            Log.Error("Sora|ReactiveApiManager", "api time out");
            return null;
        }

        #endregion
    }
}