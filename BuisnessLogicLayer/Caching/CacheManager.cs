using ContractEntities.Entities;
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

namespace BuisnessLogicLayer.Caching
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
        private readonly IDictionary<string, int> _objectCacheTimeDictionary;

        public CacheManager(IDistributedCache cache, IConfiguration configuration, ILogger<CacheManager> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
            _objectCacheTimeDictionary = new Dictionary<string, int>()
            {
                {"ContractEntities.Entities.Holiday", 3600},
                {"ContractEntities.Entities.Country", 3600},
                {"ContractEntities.Entities.LongWeekEnd", 3600},
                {"ContractEntities.Entities.ApplicationUser", 60},
                {"System.Collections.Generic.IEnumerable`1[ContractEntities.Entities.Holiday]", 3600},
                {"System.Collections.Generic.IEnumerable`1[ContractEntities.Entities.Country]", 3600},
                {"System.Collections.Generic.IEnumerable`1[ContractEntities.Entities.LongWeekEnd]", 3600},
                {"System.Collections.Generic.IEnumerable`1[ContractEntities.Entities.ApplicationUser]", 60},
            };

        }

        public async Task AddRecord<T>(T data, string key)
        {
            var cacheTime = GetObjectCacheTime(typeof(T));
            if (cacheTime > 0)
            {
                var cacheExpireTime = new TimeSpan(0, 0, cacheTime);
                _logger.LogInformation($"{typeof(T).ToString()} cache time :{cacheTime} sec");
                await _cache.SetRecordAsync<T>(key, data, cacheExpireTime);
            }
            else
            {
                _logger.LogInformation($"{typeof(T).ToString()} cache time :{cacheTime} sec");
                await _cache.SetRecordAsync<T>(key, data);
            }

        }

        public string GenerateCacheKey(List<Object> callerParams, [CallerMemberName] string callerName = "")
        {
            string appVersion = _configuration["Version"];
            string key;
            if (callerParams == null)
            {
                key = $"{callerName} {appVersion}";
            }
            else
            {
                List<string> paramsToString = callerParams.Select(s => s.ToString()).ToList();
                key = $"{callerName} {string.Join(",", paramsToString)} {appVersion}";
            }
            return key;
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

        public int GetObjectCacheTime(Type type)
        {
            int value = -1;
            _objectCacheTimeDictionary.TryGetValue(type.ToString(), out value);
            return value;

        }
    }
}
