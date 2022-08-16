using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FJournalLib.Extensions
{
    public static class DayzExtension
    {
        public static bool IsToday(this string day)
        {
            var today = DateTime.Today;

            return Regex.IsMatch(day, @"^.+_" + $"{today.Year}{today.Month}{today.Day}" + @"$");
        }

        public static string FormTodayCollectionName(this string dbName)
        {
            var todayFormatted = DateTime.Today.ToString("yyyyMMdd");

            string todayCollectionName = string.Concat(dbName, '_', todayFormatted);

            return todayCollectionName;
        }
    }
}
