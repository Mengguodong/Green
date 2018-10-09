using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RedisManager
    {
        public static string[] writeRedis = new string[] { Auxiliary.ConfigKey("WriteRedis") };
        public static string[] readOnlyRedis = new string[] { Auxiliary.ConfigKey("ReadOnlyRedis") };


        public static PooledRedisClientManager redisPoolManager;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RedisManager()
        {
            CreateRedisPool();
        }

        /// <summary>
        /// 创建redis连接池
        /// </summary>
        /// <returns></returns>
        private static void CreateRedisPool()
        {
            // 支持读写分离，均衡负载   
            redisPoolManager = new PooledRedisClientManager(writeRedis, readOnlyRedis, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 50, // “写”链接池链接数   
                MaxReadPoolSize = 50, // “读”链接池链接数   
                AutoStart = true,
            }); 
        }


        /// <summary>
        /// 获取reids写入连接
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetRedisWriteClient()
        {
            if (redisPoolManager == null)
            {
                CreateRedisPool();//如果连接池未被创建，则创建连接池
            }
            return redisPoolManager.GetClient();//返回一个写入连接
        }

        /// <summary>
        /// 获取redis只读链接
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetRedisReadOnlyClient()
        {
            if (redisPoolManager == null)
            {
                CreateRedisPool();//如果连接池未被创建，则创建连接池
            }
            return redisPoolManager.GetReadOnlyClient();//返回一个只读连接
        }


    }
}
