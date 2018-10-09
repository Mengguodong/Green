using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 公共MQ消息队列
    /// 时间：2015年7月7日9:36:22
    /// </summary>
    public class SendMessage
    {
        /// <summary>
        /// 要放到配置文件中
        /// </summary>
        // private readonly string queueName ="@"+GetConfig.GetConfigString("MQPath"); 
        // "FormatName:Direct=TCP:192.168.11.22\\private$\\ZHTXMesQueue";

        //private readonly string queueName = "FormatName:Direct=TCP:" + GetConfig.GetConfigString("MQIpAddress") + "\\private$\\"+GetConfig.GetConfigString("MQName");

        MessageQueue mesqueue = null;
        /// <summary>
        /// 创建消息队列
        /// </summary>
        public SendMessage()
        {
            string queueName = "";

            string mqIpaddress = Auxiliary.ConfigKey("MQIpAddress");
            string mqname = Auxiliary.ConfigKey("MQName");
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
            // mesqueue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
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
                try
                {
                    //同时发送消息到OpenSearch消息服务器
                    OpenSearchMessage openSearchMsg = new OpenSearchMessage();
                    openSearchMsg.SendMes(type, goodsId);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("方法名：SendMessage.SendMes" + "wzh" + "发送OpenSearch消息队列异常:" + ex.Message + "  " + goodsId);
                    //LogHelper.WriteInfo(typeof(SendMessage), "方法名：SendMessage.SendMes" + "wzh" + "发送OpenSearch消息队列异常:" + ex.Message + "  " + goodsId);
                }

            }
            catch(Exception ex)
            {
                //需要处理异常
                LogHelper.Error("方法名：SendMessage.SendMes" + "wzh" + "发送消息队列异常:" + ex.Message + "  " + goodsId);
                //LogHelper.WriteInfo(typeof(SendMessage), "方法名：SendMessage.SendMes" + "wzh" + "发送消息队列异常:" + ex.Message + "  " + goodsId);
            }
        }
        /// <summary>
        /// 品牌库变更发布修改商品索引消息
        /// 创建人：zhuzh
        /// 创建时间：2014-12-09
        /// </summary>
        /// <param name="goodsIds"></param>
        public static void SendMessageToGoods(List<int> goodsIds)
        {
            if (goodsIds != null && goodsIds.Count > 0)
            {
                foreach (var item in goodsIds)
                {
                    new Common.SendMessage().SendMes("Update", item.ToString()); //商品改变
                }
            }
        }
    }
}
