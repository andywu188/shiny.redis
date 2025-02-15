﻿using NewLife.Caching;
using NewLife.Caching.Models;
using Shiny.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiny.Redis
{
    /// <summary>
    /// 可重复消费队列
    /// </summary>
    public partial class ShinyRedis : IRedisCacheManager
    {
        /// <inheritdoc/>
        public RedisStream<T> GetRedisStream<T>(string key)
        {
            return redisConnection.GetStream<T>(key);
        }

        /// <inheritdoc/>
        public RedisStream<T> GetSteamQueue<T>(string key, string group = "", string consumer = "", bool fromLastOffset = false)
        {
            var queue = GetRedisStream<T>(key);
            if (!string.IsNullOrEmpty(group))
            {
                queue.SetGroup(group);
            }
            if (!string.IsNullOrEmpty(consumer))
            {
                queue.Consumer = consumer;
            }
            if (fromLastOffset)
            {
                queue.FromLastOffset = true;
            }
            return queue;
        }

        /// <inheritdoc/>
        public RedisStream<T> GetAutoSteamQueue<T>(string key, string group, string consumer = "")
        {
            var queue = GetRedisStream<T>(key);
            queue.SetGroup(group);
            if (!string.IsNullOrEmpty(consumer))
            {
                queue.Consumer = consumer;
            }
            var gorups = queue.GetGroups().Select(it => it.Name).ToList();//获取所有消费组
            if (!group.Contains(group))//如果消费组存在
            {
                queue.FromLastOffset = true;
            }
            return queue;
        }

        /// <inheritdoc/>
        public string AddSteamQueue<T>(string key, T value)
        {
            var queue = redisConnection.GetStream<T>(key);
            return queue.Add(value);
        }

        /// <inheritdoc/>
        public List<T> SteamQueueTake<T>(string key, int count)
        {
            var queue = redisConnection.GetStream<T>(key);
            return queue.Take(count).ToList();
        }

        /// <inheritdoc/>
        public T SteamQueueTakeOne<T>(string key, int timeout = 0)
        {
            var queue = redisConnection.GetStream<T>(key);
            return queue.TakeOne(timeout);
        }

        /// <inheritdoc/>
        public async Task<T> SteamQueueTakeOneAsync<T>(string key, int timeout = 1)
        {
            var queue = redisConnection.GetStream<T>(key);
            return await queue.TakeOneAsync(timeout);
        }

        /// <inheritdoc/>
        public Task<Message> SteamQueueTakeMessageAsync<T>(string key, int timeout = 1)
        {
            var queue = redisConnection.GetStream<T>(key);
            return queue.TakeMessageAsync(timeout);
        }

        /// <inheritdoc/>
        public async Task<IList<Message>> SteamQueueTakeMessagesAsync<T>(string key, int timeout = 1)
        {
            var queue = redisConnection.GetStream<T>(key);
            return await queue.TakeMessagesAsync(timeout);
        }

        /// <inheritdoc/>
        public int SteamQueueAcknowledge<T>(string key, string[] keys)
        {
            var queue = redisConnection.GetStream<T>(key);
            return queue.Acknowledge(keys);
        }


    }
}
