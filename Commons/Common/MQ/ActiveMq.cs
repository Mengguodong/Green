using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{

    #region 发送消息(旧)
    /// <summary>
    /// 发送MQ消息(单例)
    /// </summary>
    public class ActiveMq
    {

        private static readonly ActiveMq mqSingletion = new ActiveMq();

        //工厂和连接
        private IConnectionFactory factory = null;
        private IConnection connection = null;
        private ActiveMq()
        {
            try
            {
                //创建工厂
                factory = new ConnectionFactory(Auxiliary.ConfigKey("MQAddress"));
                //创建连接
                connection = factory.CreateConnection();

            }
            catch (Exception ex)
            {
                LogHelper.Error("单例ActiveMQ连接错误:" + ex.Message);
            }
        }

        //单例模式出口
        public static ActiveMq GetActiveMq()
        {
            return mqSingletion;
        }
        /// <summary>
        /// 消息发送（单个）
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="entity">消息数据</param>
        /// <returns>结果(0失败1成功)</returns>
        public int SendActiveMQMessage(MessageQueueName queueName, MessageEntity entity)
        {
            int msgResult = 0;  //消息结果
            string msgEntity = "";//文本消息

            //判断是否发送消息
            if (entity != null)
            {
                try
                {
                    //创建回话
                    using (ISession sesssion = connection.CreateSession())
                    {
                        //实体消息序列化文本消息
                        msgEntity = JsonConvertTool.SerializeObject(entity);

                        //创建生产者
                        IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName.ToString());
                        IMessageProducer producer = sesssion.CreateProducer(destination);
                        //生产消息
                        ITextMessage _message = producer.CreateTextMessage(msgEntity);

                        //发送消息(持久化)
                        producer.Send(_message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                    msgResult = 1;
                }
                catch (Exception ex)
                {
                  //  LogHelper.WriteLog(typeof(ActiveMq), "方法名：SendActiveMQMessage发送消息队列异常(单个):", Engineer.maq, entity, ex);
                }
            }
            return msgResult;
        }
    }
    #endregion

    #region 发送消息(新)(泛型)
    /// <summary>
    /// 发送MQ消息(单例)(泛型)
    /// </summary>
    public class ActiveMq<T>
    {

        private static readonly ActiveMq<T> mqSingletion = new ActiveMq<T>();

        //工厂和连接
        private IConnectionFactory factory = null;
        private IConnection connection = null;
        private ActiveMq()
        {
            try
            {
                //创建工厂
                factory = new ConnectionFactory(Auxiliary.ConfigKey("MQAddress"));
                //创建连接
                connection = factory.CreateConnection();

            }
            catch (Exception ex)
            {
                LogHelper.Error("单例ActiveMQ连接错误:" + ex.Message);
            }
        }

        //单例模式出口
        public static ActiveMq<T> GetActiveMq()
        {
            return mqSingletion;
        }
        /// <summary>
        /// 消息发送（单个）
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="entity">消息数据</param>
        /// <returns>结果(0失败1成功)</returns>
        public int SendActiveMQMessage(MessageQueueName queueName, T entity)
        {
            int msgResult = 0;  //消息结果
            string msgEntity = "";//文本消息

            //判断是否发送消息
            if (entity != null)
            {
                try
                {
                    //创建回话
                    using (ISession sesssion = connection.CreateSession())
                    {
                        //实体消息序列化文本消息
                        msgEntity = JsonConvertTool.SerializeObject(entity);

                        //创建生产者
                        IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName.ToString());
                        IMessageProducer producer = sesssion.CreateProducer(destination);
                        //生产消息
                        ITextMessage _message = producer.CreateTextMessage(msgEntity);

                        //发送消息(持久化)
                        producer.Send(_message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
                    }
                    msgResult = 1;
                }
                catch (Exception ex)
                {
                   // LogHelper.WriteLog(typeof(ActiveMq), "方法名：SendActiveMQMessage发送消息队列异常(单个):", Engineer.maq, entity, ex);
                }
            }
            return msgResult;
        }
    }
    #endregion

    #region 处理消息(新)(泛型)
    /// <summary>
    /// 消息基础（抽象类)
    /// </summary>
    public abstract class BaseBll<T>
    {
        /// <summary>
        /// API地址
        /// </summary>
        public string WebApi = Auxiliary.ConfigKey("WebApi");
        /// <summary>
        /// 主方法
        /// </summary>
        public abstract ResultEntity MainMethod(T entity);
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    public class HandleActiveMq<T>
    {
        #region 变量定义

        #region 服务变量
        /// <summary>
        /// 处理消息多线程集合
        /// </summary>
        private List<Thread> thdList = new List<Thread>();
        /// <summary>
        /// 接受消息集合
        /// </summary>
        private List<T> msgList = new List<T>();
        #endregion

        #region MQ变量
        /// <summary>
        /// 定义MQ工厂和连接
        /// </summary>
        private IConnectionFactory _factory = null;
        private IConnection _conn = null;
        #endregion

        #region Config配置变量

        /// <summary>
        /// MQ连接地址
        /// </summary>
        private string MQAddress = Auxiliary.ConfigKey("MQAddress");

        /// <summary>
        /// 处理消息线程数
        /// </summary>
        private int HandleThreadNum = Convert.ToInt32(Auxiliary.ConfigKey("HandleThreadNum"));
        /// <summary>
        /// 处理消息堆积数
        /// </summary>
        private int ReceiveThresholdNum = Convert.ToInt32(Auxiliary.ConfigKey("ReceiveThresholdNum"));

        /// <summary>
        /// 处理消息错误,是否重回队列（说明：重新发送到消息队列）（值：1是0否））
        /// </summary>
        private bool ErrorRepeatHandle = Convert.ToInt32(Auxiliary.ConfigKey("ErrorRepeatHandle")) == 1;
        /// <summary>
        /// 处理消息错误,线程睡眠时间（单位：1000=1秒）
        /// </summary>
        private int ErrorStopTime = Convert.ToInt32(Auxiliary.ConfigKey("ErrorStopTime"));

        #endregion

        #endregion

        #region  构造函数
        /// <summary>
        /// 设置一个锁对象
        /// </summary>
        private object _MyLock = new object();
        /// <summary>
        /// 设置基础抽象类
        /// </summary>
        private BaseBll<T> _BaseBll = null;
        /// <summary>
        /// 设置消息队列名称
        /// </summary>
        private MessageQueueName _MessageQueueName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bll"></param>
        public HandleActiveMq(BaseBll<T> baseBll, MessageQueueName messageQueueName)
        {
            this._BaseBll = baseBll;
            this._MessageQueueName = messageQueueName;
        }
        #endregion

        #region 接收消息
        /// <summary>
        /// 接收消息
        /// </summary>
        private void ReceiveMessage()
        {
            //创建工厂
            _factory = new ConnectionFactory(MQAddress);
            //创建连接
            _conn = _factory.CreateConnection();
            //设置客户端ID
            _conn.ClientId = Guid.NewGuid().ToString().Replace("_", " ").ToLower();
            _conn.Start();
            //创建会话
            ISession sessions = _conn.CreateSession(AcknowledgementMode.AutoAcknowledge);
            IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(_MessageQueueName.ToString());
            //创建消费者
            IMessageConsumer consumer = sessions.CreateConsumer(destination);
            //异步接收消息
            consumer.Listener += new MessageListener(Consumer_Listener);
        }

        /// <summary>
        /// 接收消息事件
        /// </summary>
        /// <param name="message"></param>
        private void Consumer_Listener(IMessage message)
        {
            ITextMessage itMsg = (ITextMessage)message;
            //判断是否批量接收消息
            if (!string.IsNullOrEmpty(itMsg.Text))
            {
                //批量接收消息
                RecueveMesToList(itMsg.Text);
                //批量接收消息至某阀值停止MQ连接
                if (msgList.Count >= ReceiveThresholdNum)
                {
                    _conn.Stop();
                    LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "MQ连接停止！");
                }
            }
        }

        /// <summary>
        /// 批量接收消息
        /// </summary>
        /// <returns></returns>
        private T[] RecueveMesToList(string strMsg)
        {
            Monitor.Enter(_MyLock);
            T[] arrMsg = null;
            try
            {
                //写入接收消息
                if (!string.IsNullOrEmpty(strMsg))
                {
                    //文本转换实体
                    var msgEntity = JsonConvert.DeserializeObject<T>(strMsg);
                    //集合不存在追加实体
                    if (!msgList.Contains(msgEntity))
                    {
                        msgList.Add(msgEntity);
                    }
                }
                //读取接收消息
                else
                {
                    //存在接收消息
                    if (msgList.Count > 0)
                    {
                        //通过集合数量定义数组
                        arrMsg = new T[msgList.Count];
                        //集合数据复制到数组中
                        msgList.CopyTo(arrMsg);
                        //清楚集合
                        msgList.Clear();
                    }
                    //没有接收消息
                    else
                    {
                        //MQ连接是否开启
                        if (_conn != null && (!_conn.IsStarted))
                        {
                            //MQ连接开启
                            _conn.Start();
                            LogHelper.WriteInfo(typeof(HandleActiveMq<T>), Thread.CurrentThread.Name + ":MQ连接启动！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "批量接收消息List中异常：" + ex.StackTrace);
            }
            finally
            {
                Monitor.Exit(_MyLock);
            }
            return arrMsg;
        }
        #endregion

        #region 处理消息
        /// <summary>
        /// 批量处理消息
        /// </summary>
        private void HandleMesToList()
        {
            try
            {
                //死循环处理接收消息
                while (true)
                {
                    #region 批量处理消息
                    //批量取接收消息处理
                    T[] arrMsg = RecueveMesToList(null);
                    //是否有需要处理消息
                    if (arrMsg != null && arrMsg.Length > 0)
                    {
                        //批量处理消息
                        foreach (var msg in arrMsg)
                        {
                            //是否要结束服务
                            if (IsEndMQ())
                            {
                                //是，发送消息
                                SendMessage(msg);
                            }
                            else
                            {
                                //否，处理消息
                                HandleMessage(msg);
                            }
                        }
                        arrMsg = null;
                    }
                    else
                    {
                        //没数据,线程睡眠2秒
                        Thread.Sleep(2000);
                    }
                    #endregion

                    #region 是否跳出循环
                    //判断是否跳出循环
                    if (IsEndMQ() && msgList.Count == 0)
                    {
                        //线程结束，线程列表删除线程
                        thdList.Remove(Thread.CurrentThread);
                        LogHelper.WriteInfo(typeof(HandleActiveMq<T>), Thread.CurrentThread.Name + "关闭！");
                        //线程结束，跳出循环
                        break;
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "处理消息异常" + ex.StackTrace);
            }
        }
        /// <summary>
        /// 最终消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void HandleMessage(T msg)
        {
            //处理消息
            ResultEntity result = _BaseBll.MainMethod(msg);

            //通过结果判断操作是否正确
            if (result != null && result.ResultCode == 1)
            {

            }
            else
            {
                //处理消息错误,再次处理判断
                if (ErrorRepeatHandle)
                {
                    //重新发送消息
                    SendMessage(msg);
                }

                //错误写日志
                LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "HandleActiveMq处理消息错误！结果：" + JsonConvertTool.SerializeObject(result) + "\n内容：" + JsonConvertTool.SerializeObject(msg));

                //错误是否睡眠
                if (ErrorStopTime > 0)
                {
                    //错误睡眠时间
                    Thread.Sleep(ErrorStopTime);
                }
            }
        }

        
        #endregion

        #region 开始结束
        /// <summary>
        /// 开始（启动接收和处理消息）
        /// </summary>
        public void StartMQ()
        {
            #region 接收消息
            ReceiveMessage();
            LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "消息接收启动成功！");
            #endregion

            #region 处理消息
            for (int i = 1; i <= HandleThreadNum; i++)
            {
                Thread handleThread = new Thread(HandleMesToList);
                handleThread.Name = "消息处理线程" + i.ToString();
                handleThread.IsBackground = true;
                handleThread.Start();

                LogHelper.WriteInfo(typeof(HandleActiveMq<T>), handleThread.Name + "启动成功！");

                //追加到线程集合
                thdList.Add(handleThread);
            }
            #endregion
        }

        /// <summary>
        /// 结束(内存中剩余的消息重发)
        /// </summary>
        public void EndMQ()
        {
            #region 消息接收关闭
            if (_conn != null)
            {
                //MQ连接关闭
                _conn.Stop();
                _conn.Close();
                //MQ工厂和连接初始化
                _factory = null;
                _conn = null;

                LogHelper.WriteInfo(typeof(HandleActiveMq<T>), "MQ连接关闭！");
            }
            #endregion

            #region 线程是否关闭
            //死循环，判断线程是否关闭
            while (true)
            {
                //跳出线程
                if (thdList.Count == 0)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            #endregion
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 验证是否要结束服务
        /// </summary>
        /// <returns></returns>
        private bool IsEndMQ()
        {
            bool bl = false;
            //判断是否要结束服务
            if (_factory == null && _conn == null)
            {
                bl = true;
            }
            return bl;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        private void SendMessage(T msg)
        {
            //重新发送消息
            ActiveMq<T>.GetActiveMq().SendActiveMQMessage(this._MessageQueueName, msg);
        }

        #endregion
    }
    #endregion
}
