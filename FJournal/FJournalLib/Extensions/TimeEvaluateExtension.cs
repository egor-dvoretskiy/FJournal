using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Extensions
{
    public static class TimeEvaluateExtension
    {        
        private static Stopwatch stopwatch = new Stopwatch();

        public static T EvaluateTime<T>(this object context, Func<T> function, out double EvaluatedTime)
        {
            _ = context;

            stopwatch.Restart();

            T result = function();

            stopwatch.Stop();

            EvaluatedTime = stopwatch.Elapsed.TotalMilliseconds;

            return result;
        }
    }
}
