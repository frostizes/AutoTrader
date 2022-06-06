using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils.CacheHelper
{
    public interface ICacheManager
    {
        public Task<T> GetRecords<T>();
        public Task<T> GetRecord<T>(string key);
        public Task AddRecord<T>(T data, string key);

        /// <summary>
        /// careful, you pass a list of objects that we cast as a string later, so the object must override ToString() method
        /// </summary>
        /// <param name="callerParams">the list of params that the method takes</param>
        /// <param name="callerName">the name of the method who called generateCacheKey, leave if blank by default</param>
        /// <returns></returns>
        public string GenerateCacheKey(params string[] arguments);
        public string GenerateCacheKey();
        public Task<T> CheckCache<T>(Func<T> dbCall, string cacheKey);
    }
}
