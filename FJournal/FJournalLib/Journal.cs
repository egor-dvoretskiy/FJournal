using FJournalLib.Enums;
using FJournalLib.Interfaces;
using FJournalLib.Models;
using FJournalLib.Repositories;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FJournalLib
{
    public class Journal
    {
        private JournalStateLocal localState = JournalStateLocal.Enabled;
        private static JournalStateGlobal globalState = JournalStateGlobal.Enabled;

        private readonly IRepository<DBRecord> _recordRepository;
        private readonly PerformanceCounter _totalCpuCounter;

        public Journal()
        {
            this._recordRepository = new MongoRecordRepository();
            this._totalCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
        }

        public static void SetJournalGlobalState(JournalStateGlobal globalStateInput) => globalState = globalStateInput;

        public void SetJournalLocalState(JournalStateLocal localStateInput) => localState = localStateInput;

        public void Note(TRecord inputRecord,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] uint lineNumber = 0)
        {
            if (globalState != JournalStateGlobal.Enabled && localState != JournalStateLocal.Enabled)
                return;

            long memoryUsageMb = this.GetMemoryUsageInMb();
            float cpuUsagePercentage = this.GetTotalCpuUsage();

            DBRecord record = new DBRecord()
            {
                TimeStamp = DateTime.Now,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                TotalCpuUsage = cpuUsagePercentage,
                PrivateMemoryUsage = memoryUsageMb,
            };

            try
            {
                record.Message = inputRecord.Message;
                record.LogType = inputRecord.LogType;
                record.LogSource = Enums.LogSource.External;

                this._recordRepository.Create(record);
            }
            catch (Exception exception)
            {
                record.Message = exception.Message;
                record.LogType = Enums.LogType.Error;
                record.LogSource = Enums.LogSource.Inner;

                this._recordRepository.Create(record);
            }
        }

        private long GetMemoryUsageInMb() => Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024; // converting bytes to mb

        private float GetTotalCpuUsage() => this._totalCpuCounter.NextValue();
    }
}
