using FJournalLib.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Models
{
    public class DBRecord
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public LogSource LogSource { get; set; }

        public LogType LogType { get; set; }

        public string Message { get; set; } = string.Empty;

        public string CallerMemberName { get; set; } = "-";

        public string CallerFilePath { get; set; } = "-";

        public uint CallerLineNumber { get; set; } = 0;

        public float TotalCpuUsage { get; set; }

        public long PrivateMemoryUsage { get; set; }
    }
}
