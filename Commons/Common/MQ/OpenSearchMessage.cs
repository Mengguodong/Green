using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Threading.Tasks;

namespace Common
{
    public class OpenSearchMessage
    {

        MessageQueue mesqueue = null;
        public OpenSearchMessage()
        {
            string queueName = "";
            string mqIpaddress = Auxiliary.ConfigKey("OpenSearchMQIPAddress");
            string mqname = Auxiliary.ConfigKey("OpenSearchMQName");
            if (mqIpaddress != ".")
            {
                //本机
                queueName = "FormatName:Direct=TCP:" + mqIpaddress + "\\private$\\" + mqname;
            }
            else
            {
                queueName = ".\\private$\\" + mqname;
            }
            mesqueue = new MessageQueue(queueName);
        }

        /// <summary>
        /// 发送消息
        /// type 类型 添加商品"Add", 修改商品"Update",删除"Del"
        /// 商品上下架，修改商品状态，库存，统一发送"Update"
        /// </summary>
        public void SendMes(string type, string goodsId)
        {
            Message m = new Message();
            try
            {
                m.Label = "LuceneMes";
                m.Body = type + "|" + goodsId;
                this.mesqueue.Send(m);
            }
            catch (Exception ex)
            {
                //需要处理异常
                LogHelper.WriteInfo(typeof(SendMessage), "方法名：OpenSearchMessage.SendMes" + "wzh" + "发送消息队列异常:" + ex.Message + "  " + goodsId);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMes(string messageBody)
        {
            Message m = new Message();
            try
            {
                m.Label = "LuceneMes";
                m.Body = messageBody;
                this.mesqueue.Send(m);
            }
            catch (Exception ex)
            {
                //需要处理异常
                LogHelper.WriteInfo(typeof(OpenSearchMessage), "方法名：OpenSearchMessage.SendMes" + "wzh" + "发送消息队列异常:" + ex.Message + ":" + messageBody);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="doType">操作类型  ADD:增加 UPDATE:修改 DEL:删除</param>
        /// <param name="keyValue">主键字段的值(商品ID)</param>
        /// <param name="messageType">消息类型  1:普通商品（原平台商品）  2:掌合工厂商品  其他值:全部按普通商品处理</param>
        public void SendMes(OpenSeachMessageDoType doType, string keyValue, OpenSearchMessageType messageType)
        {
            Message m = new Message();
            try
            {
                m.Label = "LuceneMes";
                m.Body = doType.ToString() + "|" + keyValue + "|" + (int)messageType;
                this.mesqueue.Send(m);
            }
            catch (Exception ex)
            {
                //需要处理异常
                LogHelper.WriteInfo(typeof(SendMessage), "方法名：OpenSearchMessage.SendMes" + "wzh" + "发送消息队列异常:" + ex.Message + "  " + keyValue);
            }
        }


        /// <summary>
        /// 索引操作类型枚举
        /// </summary>
        public enum OpenSeachMessageDoType
        {
            /// <summary>
            /// 创建索引
            /// </summary>
            ADD = 1,
            /// <summary>
            /// 修改索引
            /// </summary>
            UPDATE = 2,
            /// <summary>
            /// 删除索引
            /// </summary>
            DELETE = 3
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public enum OpenSearchMessageType
        {
            /// <summary>
            /// 超市、总部、服务站、供货商平台操作商品索引的消息
            /// </summary>
            SupermarketGoods = 1,

            /// <summary>
            /// 掌合工厂商品消息
            /// </summary>
            FactoryGoods = 2,

            /// <summary>
            /// 营销：订货会套餐商品消息
            /// </summary>
            SalesGoods = 3
        }


    }
}
