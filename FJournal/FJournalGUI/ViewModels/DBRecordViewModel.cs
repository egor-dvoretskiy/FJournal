using FJournalLib.Enums;
using FJournalLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalGUI.ViewModels
{
    public class DBRecordViewModel
    {
        private readonly DBRecord _record;

        public DBRecordViewModel(DBRecord record)
        {
            this._record = record;
        }

        public DateTime TimeStamp 
        { 
            get
            {
                return _record.TimeStamp;
            }
        }

        public LogSource LogSource 
        { 
            get
            {
                return this._record.LogSource;
            }
        }

        public LogType LogType
        {
            get
            {
                return this._record.LogType;
            }
        }

        public string? Message
        {
            get
            {
                return this._record.Message;
            }
        }

        public string CallerMemberName
        {
            get
            {
                return this._record.CallerMemberName;
            }
        }

        public string CallerFilePath
        {
            get
            {
                return this._record.CallerFilePath;
            }
        }

        public uint CallerLineNumber
        {
            get
            {
                return this._record.CallerLineNumber;
            }
        }

        public float TotalCpuUsage
        {
            get
            {
                return this._record.TotalCpuUsage;
            }
        }

        public long PrivateMemoryUsage
        {
            get
            {
                return this._record.PrivateMemoryUsage;
            }
        }
    }
}
