using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Helpers
{
    public static class CacheHelper
    {
        public static void Save(string key, object data)
        {
            MemoryCache.Default.Add(new CacheItem(key, data), new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(15) });
        }

        public static void Delete(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public static T Get<T>(string key)
        {
            return (T)MemoryCache.Default.Get(key);
        }
    }
}
