using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    #region 消息实体
    /// <summary>
    /// 消息实体(旧List)
    /// </summary>
    [Serializable]
    public class MessageEntity
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 消息值
        /// </summary>
        public List<string> MessageValue { get; set; }
    }

    /// <summary>
    /// 消息实体(新T)
    /// </summary>
    [Serializable]
    public class MessageEntity<T>
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; }
        /// <summary>
        /// 消息值
        /// </summary>
        public T MessageValue { get; set; }
    }
    #endregion

    #region 结果实体
    /// <summary>
    /// 结果消息实体(新T)
    /// </summary>
    [Serializable]
    public class ResultEntity
    {
        /// <summary>
        /// 结果消息码1成功2失败（强制要求）
        /// </summary>
        public int ResultCode { get; set; }


        public string MethodName { get; set; }

        /// <summary>
        /// 结果消息值
        /// </summary>
        public Object ResultValue { get; set; }
    }
    #endregion

    #region 消息类型
    /// <summary>
    /// 消息类型
    /// </summary>
    [Serializable]
    public enum MessageType
    {
        #region 处理业务类型（范围:1000-1099）

        #endregion

        #region 监控消息类型（范围:1100-1199）
        /// <summary>
        /// 监控
        /// </summary>
        Monitor = 1100,
        /// <summary>
        /// API接口的监控
        /// </summary>
        Monitor_API = 1101,
        #endregion

        #region 搜索消息类型（范围:1200-1299）

        #endregion

        #region 日志消息类型（范围:1300-1399）

        #endregion

        #region 缓存消息类型（范围:1400-1499）

        #endregion

        #region 支付消息类型（范围:1500-1599）

        /// <summary>
        /// 支付回调
        /// </summary>
        PaycallBack = 1501,

        #endregion

        #region 记录消息类型（范围1600-1699）
        Oper_Record = 1600,
        #endregion

        #region 营销红包类型（范围:1700-1799）
        RedPackage = 1700,

        //口令红包
        PwdRedPackage = 1701

        #endregion

    }
    #endregion

    #region 队列名称
    /// <summary>
    /// 队列名称
    /// </summary>
    public enum MessageQueueName
    {
        /// <summary>
        /// 处理业务的消息队列
        /// </summary>
        zh_business,
        /// <summary>
        /// 处理监控的消息队列
        /// </summary>
        zh_monitor,
        /// <summary>
        /// 处理搜索的消息队列
        /// </summary>
        zh_search,
        /// <summary>
        /// 处理日志的消息队列
        /// </summary>
        zh_log,
        /// <summary>
        /// 处理缓存的消息队列
        /// </summary>
        zh_redis,
        /// <summary>
        /// 支付回调的消息队列
        /// </summary>
        zh_pay,
        /// <summary>
        /// 操作记录的消息队列
        /// </summary>
        zh_record,
        /// <summary>
        /// 营销
        /// </summary>
        zh_sale,
        /// <summary>
        ///测试支付回调消息队列
        /// </summary>
        test_pay,
    }
    #endregion


}
