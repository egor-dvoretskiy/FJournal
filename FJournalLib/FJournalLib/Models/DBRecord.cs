using FJournalLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Models
{
    internal class DBRecord
    {
        public uint Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public LogSource LogSource { get; set; }

        public LogType LogType { get; set; }

        public string? Message { get; set; }
    }
}
