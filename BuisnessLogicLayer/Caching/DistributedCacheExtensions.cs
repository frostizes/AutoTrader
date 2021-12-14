using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuisnessLogicLayer.Caching
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// this is the method responsible for adding records to cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">this param is the cache itself defined in the startup file so we can do cache.SetRecordAsync()</param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="unusedExpireTime">if you unaccesed cache in unusedExpireTime time do something</param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
            Debug.WriteLine("written");
        }

        /// <summary>
        /// this method is responsible for retrieving data from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if(jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
