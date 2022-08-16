using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Extensions
{
    public static class MemoizerExtension
    {
        private static ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();

        public static TResult Memoized<T1, TResult> (
            this object context,
            T1 arg,
            Func<T1, TResult> func,
            [CallerMemberName] string? cacheKey = null)
            where T1 : notnull
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (cacheKey == null)
            {
                throw new ArgumentNullException(nameof(cacheKey));
            }

            var methodCache = (ConcurrentDictionary<T1, TResult>)cache.GetOrAdd(cacheKey, _ => new ConcurrentDictionary<T1, TResult>());

            return methodCache.GetOrAdd(arg, func);
        }

        public static void RefreshMemoizer() => cache.Clear();
    }
}
