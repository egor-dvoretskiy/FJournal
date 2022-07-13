using FJournalLib.Models;
using FJournalLib.Repositories;
using MongoDB.Driver;
using System;
using System.Linq;

namespace FJournalLib
{
    public class Journal
    {
        private static bool isEnabled = false;
        private readonly MongoRecordRepository recordRepository;

        public Journal()
        {
            Enable();
            this.recordRepository = new MongoRecordRepository();
        }

        public static void Enable() => isEnabled = true;

        public static void Disable() => isEnabled = false;

        public void Note(TRecord record)
        {
            if (!isEnabled)
                return;

            try
            {
                this.recordRepository.Create(new DBRecord()
                {
                    Id = this.GeneratePositionForRecord(),
                    Message = record.Message,
                    TimeStamp = DateTime.Now,
                    LogSource = Enums.LogSource.External,
                    LogType = record.LogType,
                });
            }
            catch (Exception exception)
            {
                this.recordRepository.Create(new DBRecord()
                {
                    Id = this.GeneratePositionForRecord(),
                    Message = exception.Message,
                    TimeStamp = DateTime.Now,
                    LogSource = Enums.LogSource.Inner,
                    LogType = Enums.LogType.Error
                });
            }
        }

        private uint GeneratePositionForRecord()
        {
            var collection = this.recordRepository.GetCollectionByName();

            if (collection.EstimatedDocumentCount() == 0)
                return 0;

            var position = collection.AsQueryable<DBRecord>()
                                        .Select(x => x.Id)
                                        .Max();

            return ++position;
        }
        
    }
}
