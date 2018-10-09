using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// redis帮助类
    /// 2015年3月31日
    /// </summary>
    public class RedisHelper
    {

        #region 写入Redis
        /// <summary>
        /// 向redis中添加一条数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">对象</param>
        /// <param name="expireTime">过期时间</param>
        public void AddValueToRedis<T>(string key, T t, DateTime? expireTime)
        {
            try
            {
                using (IRedisClient client = RedisManager.GetRedisWriteClient())
                {
                    client.Add<T>(key, t);
                    if (expireTime.HasValue)
                    {
                        client.ExpireEntryAt(key, expireTime.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog<T>(this.GetType(), "AddValueToRedis;键：" + key + "", "鲍晨", t, ex);
            }
        }
        public void AddValueToRedis<T>(string key, T t, TimeSpan? timeSpan)
        {
            try
            {
                using (IRedisClient client = RedisManager.GetRedisWriteClient())
                {
                    if (timeSpan.HasValue)
                    {
                        client.Add<T>(key, t, timeSpan.Value);
                    }
                    else
                    {
                        client.Add<T>(key, t);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog<T>(this.GetType(), "AddValueToRedis 键：" + key + "", "鲍晨", t, ex);
            }
        }
        /// <summary>
        /// 添加一项item到List上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expireTime"></param>
        public void AddItemToList<T>(string key, T t, DateTime? expireTime)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.AddItemToList(typedClient.Lists[key], t);
                if (expireTime.HasValue)
                {
                    typedClient.ExpireEntryAt(key, expireTime.Value);
                }
            }
        }

        #endregion

        #region 查询Redis
        /// <summary>
        /// 从redis中获取value
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
                return client.Get<T>(key);
            }
        }
        /// <summary>
        /// 从redis中获取value集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="keyList">redis键值</param>
        /// <returns></returns>
        public List<T> GetValueList<T>(List<string> keyList)
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
                List<T> list = new List<T>();
                foreach (var item in keyList)
                {
                    list.Add(client.Get<T>(item));
                }
                // return client.Get<T>(key);
                return list;
            }
        }

        /// <summary>
        /// 模糊查询key 
        /// 
        /// 2016-07-28  14:27:33
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> GetLikeKeys(string key)
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
              return  client.SearchKeys(key + "*");
            }
        }

        #endregion

        #region 删除
        /// <summary>
        /// 根据键删除redis信息
        /// </summary>
        /// <param name="key"></param>
        public bool Delete(string key)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                return client.Remove(key);
            }
        }


        #endregion

        #region 判断key是否存在

        public bool CheckKey(string key)
        {
            //return false;
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                return client.ContainsKey(key);
            }
        }

        #endregion

        /// <summary>
        /// 设置过期时间 
        /// 
        /// 2016-02-24  15:26:30
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="sp">时间间隔</param>
        public void SetKeyExpire(string key, TimeSpan sp)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                client.ExpireEntryIn(key, sp);
            }
        }
        /// <summary>
        /// 设置过期时间 
        /// 
        /// 2016-02-24  15:46:14
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dt">日期</param>
        public void SetKeyExpire(string key, DateTime dt)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                client.ExpireEntryAt(key, dt);
            }
        }
        /// <summary>
        /// 指定主键key的value值加上amount 
        /// 
        /// 2016-03-02  17:30:45
        /// </summary>
        /// <param name="key"></param>
        /// <param name="amount"></param>
        public long Increment(string key, uint amount)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                return client.Increment(key, amount);

            }
        }
        #region Store操作
        /// <summary>
        /// 存储
        /// 2016-03-28 23:45:29 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void StoreAll<T>(List<T> entities)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.StoreAll(entities);
            }
        }

        /// <summary>
        /// 存储
        /// 2016-03-28 23:45:29 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Store<T>(T t)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.Store(t);
            }
        }


        /// <summary>
        /// 根据Ids获取Store
        ///
        /// 2016-03-29 02:08:02 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<T> GetByIds<T>(List<int> ids)
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                return typedClient.GetByIds(ids);
            }
        }

        /// <summary>
        /// 根据Id获取Store
        /// 2016-03-29 02:08:02 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public T GetById<T>(int id)
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                return typedClient.GetById(id);
            }
        }

        /// <summary>
        /// 获取所有
        /// 2016-03-28 23:45:29 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IList<T> GetAll<T>()
        {
            using (IRedisClient client = RedisManager.GetRedisReadOnlyClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                return typedClient.GetAll();
            }
        }

        /// <summary>
        /// 删除所有
        /// 2016-03-28 23:48:18 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DeleteAll<T>()
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.DeleteAll();
            }
        }


        /// <summary>
        /// 根据Ids删除
        /// 2016-03-28 23:48:18 
        /// </summary>
        /// <param name="ids"></param>
        /// <typeparam name="T"></typeparam>
        public void DeleteByIds<T>(IList<int> ids)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.DeleteByIds(ids);
            }
        }

        /// <summary>
        /// 根据Ids删除
        /// 2016-03-28 23:48:18 
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        public void DeleteById<T>(int id)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                IRedisTypedClient<T> typedClient = client.As<T>();
                typedClient.DeleteById(id);
            }
        }
        #endregion


        #region setoption

        public List<string> GetAllItemsFromSet(string setId)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                return client.GetAllItemsFromSet(setId).ToList();
            }
        }

        public void AddItemToSet(string setId, string item)
        {
            using (IRedisClient client = RedisManager.GetRedisWriteClient())
            {
                client.AddItemToSet(setId, item);
            }
        }


        #endregion
    }
}
