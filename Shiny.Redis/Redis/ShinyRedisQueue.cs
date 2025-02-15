﻿using NewLife.Caching;
using Shiny.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiny.Redis
{
    /// <summary>
    /// 普通队列 
    /// </summary>
    public partial class ShinyRedis : IRedisCacheManager
    {

        /// <inheritdoc />
        public RedisQueue<T> GetRedisQueue<T>(string key)
        {
            return redisConnection.GetQueue<T>(key) as RedisQueue<T>;
        }

        /// <inheritdoc />
        public int AddQueue<T>(string key, T[] value)
        {
            var queue = GetRedisQueue<T>(key);
            return queue.Add(value);
        }

        /// <inheritdoc />
        public int AddQueue<T>(string key, T value)
        {
            var queue = GetRedisQueue<T>(key);
            return queue.Add(value);
        }

        /// <inheritdoc />
        public List<T> GetQueue<T>(string key, int Count = 1)
        {
            var queue = GetRedisQueue<T>(key);
            var result = queue.Take(Count).ToList();

            return result;
        }

        /// <inheritdoc />
        public T GetQueueOne<T>(string key)
        {
            var queue = GetRedisQueue<T>(key);
            return queue.TakeOne(1);
        }

        /// <inheritdoc />
        public async Task<T> GetQueueOneAsync<T>(string key)
        {
            var queue = GetRedisQueue<T>(key);
            return await queue.TakeOneAsync(1);
        }

    }
}
