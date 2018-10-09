using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using Newtonsoft.Json;

namespace Dapper
{
    public partial class SqlMapper 
    {
        /// <summary>
        /// 获取配置的时间
        /// </summary>
        /// 
        public static int GetTime()
        {
            try
            {
                string Time = Auxiliary.ConfigKey("E-Time") == "" ? "1" : Auxiliary.ConfigKey("E-Time");
                return Convert.ToInt32(Time);
            }
            catch (Exception)
            {
                return 1;
            }
        }


        //obj是无法确定容量的数组类型 object[]
        private static string GetString(object obj)
        {
            if (obj == null)
            {
                return "暂无参数";
            }

            string strRst = "";
            if (obj is System.Object[])
            {
                //确定数组大小
                int iMax = (obj as Object[]).Length;
                for (int i = 0; i < iMax; i++)
                {
                    //循环取得数组内容
                    strRst += (obj as Object[]).GetValue(i).ToString();
                }
            }
            if (obj is System.Object)
            {
                strRst = JsonConvert.SerializeObject(obj);
            }
            return strRst;
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="Runtime"></param>
        /// <param name="ProjectName"></param>
        /// <param name="SqlHash"></param>
        /// <param name="SqlMsg"></param>
        /// <param name="SqlParam"></param>
        /// <param name="author"></param>
        public static void AddMessageEntity(string Runtime, string ProjectName, string SqlHash, string SqlMsg, string SqlParam, string author, string SqlCreateTime)
        {

            MessageEntity mes = new MessageEntity()
            {
                MessageType = MessageType.Monitor,
                MessageValue = new List<string>()
                     {
                            //先后顺序不要改
                            Runtime,//sql运行时间
                            ProjectName,
                            SqlHash,//sql 哈希值
                            SqlMsg,
                            SqlParam,
                            author,
                            SqlCreateTime
                      }
            };


            ActiveMq.GetActiveMq().SendActiveMQMessage(MessageQueueName.zh_monitor, mes);

        }

        /// <summary>
        /// 重载Dapper查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="author"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, Engineer author, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {

            //是否开启查询监控
            if (Common.PubConstant.IsOpenQuerySqlMonitorLog)
            {

                #region 计算时间

                Stopwatch sw = new Stopwatch();
                sw.Start();
                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, buffered ? CommandFlags.Buffered : CommandFlags.None);
                var data = QueryImpl<T>(cnn, command, typeof(T));
                sw.Stop();
                #endregion

                #region 判断执行时间 是否大于1秒
                //if (sw.Elapsed.TotalSeconds >= GetTime())
                //{
                //    #region 异步发送消息
                //    string ProjectName = "主项目";
                //    string SqlHash = sql.GetHashCode().ToString();
                //    string Runtime = sw.Elapsed.TotalSeconds.ToString();
                //    string SqlMsg = sql.Replace("\r\n", "");
                //    string SqlParam = GetString(param);

                //    AddMessageEntity(Runtime, ProjectName, SqlHash, SqlMsg, SqlParam, author.ToString(), DateTime.Now.ToString());
                //    #endregion
                //}

                #endregion

                return command.Buffered ? data.ToList() : data;

            }
            else
            {
                //没有开启监控
                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, buffered ? CommandFlags.Buffered : CommandFlags.None);
                var data = QueryImpl<T>(cnn, command, typeof(T));
                return command.Buffered ? data.ToList() : data;
            }
        }


        /// <summary>
        /// 重载Dapper ExecuteScalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(this IDbConnection cnn, string sql, Engineer author, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //是否开启非查询监控
            if (Common.PubConstant.IsOpenQuerySqlMonitorLog)
            {

                #region 计算时间
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, CommandFlags.Buffered);
                sw.Stop();
                #endregion

                #region 判断执行时间 是否大于1秒
                if (sw.Elapsed.TotalSeconds >= GetTime())
                {

                    #region 异步发送消息
                    string ProjectName = "主项目";
                    string SqlHash = sql.GetHashCode().ToString();
                    string Runtime = sw.Elapsed.TotalSeconds.ToString();
                    string SqlMsg = sql.Replace("\r\n", "");
                    string SqlParam = GetString(param);

                    AddMessageEntity(Runtime, ProjectName, SqlHash, SqlMsg, SqlParam, author.ToString(), DateTime.Now.ToString());
                    #endregion
                }
                #endregion

                return ExecuteScalarImpl<T>(cnn, ref command);

            }
            else
            {

                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, CommandFlags.Buffered);
                return ExecuteScalarImpl<T>(cnn, ref command);
            }
        }
        /// <summary>
        /// 重写Dapper Execute
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int Execute(this IDbConnection cnn, string sql, Engineer author, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //是否开启非查询监控
            if (Common.PubConstant.IsOpenNonQuerySqlMonitorLog)
            {

                #region 记录开始时间
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, CommandFlags.Buffered);
                //sw.Stop();
                #endregion

                #region 判断执行时间 是否大于1秒
                //if (sw.Elapsed.TotalSeconds >= GetTime())
                //{

                //    #region 异步发送消息
                //    string ProjectName = "主项目";
                //    //string SqlHash = sql.GetHashCode().ToString();
                //    //string Runtime = sw.Elapsed.TotalSeconds.ToString();
                //    //string SqlMsg = sql.Replace("\r\n", "");
                //    //string SqlParam = GetString(param);

                //    //AddMessageEntity(Runtime, ProjectName, SqlHash, SqlMsg, SqlParam, author.ToString(), DateTime.Now.ToString());
                //    #endregion
                //}
                #endregion


                return ExecuteImpl(cnn, ref command);
            }
            else
            {
                var command = new CommandDefinition(sql, (object)param, transaction, commandTimeout, commandType, CommandFlags.Buffered);
                return ExecuteImpl(cnn, ref command);
            }
        }

    }
}
