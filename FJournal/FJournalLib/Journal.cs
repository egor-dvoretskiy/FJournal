﻿using FJournalLib.Enums;
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
        private JournalState localState = JournalState.Enabled;
        private static JournalState globalState = JournalState.Enabled;

        private static ApplicationType applicationType = ApplicationType.Undefined;

        private readonly IRepository<DBRecord> _recordRepository;
        private readonly PerformanceCounter _totalCpuCounter;

        public Journal()
        {
            this._recordRepository = new MongoRecordRepository();
            this._totalCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
        }

        public static void SetApplicationType(ApplicationType type) => applicationType = type;

        public static void SetJournalGlobalState(JournalState globalStateInput) => globalState = globalStateInput;

        public void SetJournalLocalState(JournalState localStateInput) => localState = localStateInput;

        public void Note(TRecord inputRecord,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] uint lineNumber = 0)
        {
            if (globalState != JournalState.Enabled && localState != JournalState.Enabled)
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
                record.LogSource = LogSource.External;

                this._recordRepository.Create(record);
            }
            catch (Exception exception)
            {
                record.Message = exception.Message;
                record.LogType = LogType.Error;
                record.LogSource = LogSource.Inner;

                this._recordRepository.Create(record);
            }
        }

        private long GetMemoryUsageInMb() => Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024; // converting bytes to mb

        private float GetTotalCpuUsage() => this._totalCpuCounter.NextValue();
    }
}
