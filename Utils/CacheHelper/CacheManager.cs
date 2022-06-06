using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Utils.CacheHelper
{

    /// <summary>
    /// this class is the core of the caching system, it is responsible for the generation of cache keys, setting the cache expire time relative to objects,
    /// adding to cache and retrieving from cache.
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CacheManager> _logger;

        public CacheManager(IDistributedCache cache, IConfiguration configuration, ILogger<CacheManager> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;

        }

        public async Task AddRecord<T>(T data, string key)
        {
            //var cacheTime = 9999999999;
            //if (cacheTime > 0)
            //{
            //    var cacheExpireTime = new TimeSpan(0, 0, cacheTime);
            //    _logger.LogInformation($"{typeof(T).ToString()} cache time :{cacheTime} sec");
            //    await _cache.SetRecordAsync<T>(key, data, cacheExpireTime);
            //}
            //else
            //{
            //    _logger.LogInformation($"{typeof(T).ToString()} cache time :{cacheTime} sec");
            //    await _cache.SetRecordAsync<T>(key, data);
            //}
            var cacheExpireTime = new TimeSpan(1, 0, 0);
            await _cache.SetRecordAsync<T>(key, data, cacheExpireTime);

        }

        public string GenerateCacheKey(params string[] arguments)
        {
            StackTrace stackTrace = new StackTrace();
            var callerName = stackTrace.GetFrame(1).GetMethod().Name;
            string appVersion = _configuration["Version"];
            var concatArgument = string.Join("_", arguments);
            return $"{callerName}_{concatArgument}_{appVersion}";
        }

        public string GenerateCacheKey()
        {
            StackTrace stackTrace = new StackTrace();
            var callerName = stackTrace.GetFrame(1).GetMethod().Name;
            string appVersion = _configuration["Version"];
            return $"{callerName}_{appVersion}";
        }

        public async Task<T> CheckCache<T>(Func<T> dbCall, string cacheKey)
        {
            try
            {
                var resultFromCache = await GetRecord<T>(cacheKey);
                if (resultFromCache is null)
                {
                    _logger.LogInformation("infos retrieved from DB");
                    var resultFromDB = dbCall();
                    await AddRecord<T>(resultFromDB, cacheKey);
                    return resultFromDB;
                }
                _logger.LogInformation("infos retrieved from cache");
                return resultFromCache;
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task<T> GetRecord<T>(string key)
        {
            var result = await _cache.GetRecordAsync<T>(key);
            if (result is null)
            {
                return default(T);
            }
            else
            {
                return result;
            }
        }

        Task<T> ICacheManager.GetRecords<T>()
        {
            throw new NotImplementedException();
        }
    }
}
