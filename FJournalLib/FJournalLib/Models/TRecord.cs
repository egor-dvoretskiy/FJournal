using FJournalLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Models
{
    public struct TRecord
    {
        public LogType LogType { get; set; }

        public string? Message { get; set; }
    }
}
