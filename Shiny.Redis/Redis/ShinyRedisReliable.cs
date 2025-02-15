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
    /// 可信队列
    /// </summary>
    public partial class ShinyRedis : IRedisCacheManager
    {
        /// <inheritdoc  />
        public RedisReliableQueue<T> GetRedisReliableQueue<T>(string key)
        {
            var queue = redisConnection.GetReliableQueue<T>(key);
            return queue;
        }

        /// <inheritdoc  />
        public int RollbackAllAck(string key, int retryInterval = 60)
        {
            var queue = GetRedisReliableQueue<string>(key);
            queue.RetryInterval = retryInterval;
            return queue.RollbackAllAck();
        }

        /// <inheritdoc  />
        public T ReliableTakeOne<T>(string key)
        {
            var queue = GetRedisReliableQueue<T>(key);
            return queue.TakeOne(1);
        }

        /// <inheritdoc  />
        public async Task<T> ReliableTakeOneAsync<T>(string key)
        {
            var queue = GetRedisReliableQueue<T>(key);
            return await queue.TakeOneAsync(1);
        }

        /// <inheritdoc  />
        public List<T> ReliableTake<T>(string key, int count)
        {
            var queue = GetRedisReliableQueue<T>(key);
            return queue.Take(count).ToList();
        }

        /// <inheritdoc  />
        public int AddReliableQueueList<T>(string key, List<T> value)
        {
            var queue = redisConnection.GetReliableQueue<T>(key);
            var count = queue.Count;
            var result = queue.Add(value.ToArray());
            return result - count;
        }

        /// <inheritdoc  />
        public int AddReliableQueue<T>(string key, T value)
        {
            var queue = redisConnection.GetReliableQueue<T>(key);
            var count = queue.Count;
            var result = queue.Add(value);
            return result - count;
        }
    }
}
