using FJournalLib.Interfaces;
using FJournalLib.Models;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace FJournalLib.Repositories
{
    public class MongoRecordRepository : IRepository<DBRecord>
    {
        private static readonly string _dbName =                "FJournalDb";
        private static readonly string _connectionString =      "mongodb://localhost:27017/FJounalDb";

        private readonly IMongoDatabase _database;

        private static string todayCollectionNameCached = string.Empty;

        public MongoRecordRepository()
        {
            MongoClient client = new MongoClient(_connectionString);
            this._database = client.GetDatabase(_dbName);
        }

        public void Create(DBRecord record)
        {
            var collection = this._database.GetCollection<DBRecord>(this.GetCollectionNameForToday());
            collection.InsertOne(record);
        }

        public void Delete(ObjectId id, string collectionName = "")
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);
            collection.DeleteOne(x => x.Id == id);
        }

        public void Dispose()
        {

        }

        public DBRecord GetRecord(ObjectId id, string collectionName = "")
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);
            var record = collection.AsQueryable()
                                    .SingleOrDefault(x => x.Id == id);

            return record;
        }

        public IEnumerable<DBRecord> GetRecordsByAmount(int amountOfRecordsToGet)
        {
            List<DBRecord> records = new List<DBRecord>();

            var collectionNames = this._database.ListCollectionNames().ToList();
            collectionNames.Reverse();

            foreach(var collectionName in collectionNames)
            {
                var collection = this._database.GetCollection<DBRecord>(collectionName);

                if (records.Count < amountOfRecordsToGet)
                {
                    int amountOfRecordsToFill = Math.Abs(amountOfRecordsToGet - records.Count);
                    var amountOfItemsInCollection = collection.EstimatedDocumentCount();

                    try
                    {
                        if (amountOfRecordsToFill >= amountOfItemsInCollection)
                        {
                            records.AddRange(collection.AsQueryable());
                        }
                        else
                        {
                            records.AddRange(collection.AsQueryable().ToEnumerable().TakeLast(amountOfRecordsToFill));
                        }
                    }
                    catch (FormatException) {}                    
                }
            }

            var orderedRecordsByTimeStamp = records.OrderBy(x => x.TimeStamp);

            return orderedRecordsByTimeStamp;
        }

        public void Save()
        {

        }

        public void Update(DBRecord record)
        {
            var collection = this._database.GetCollection<DBRecord>(this.GetCollectionNameForToday());
            collection.ReplaceOne(x => x.Id == record.Id, record);
        }

        public IMongoCollection<DBRecord> GetCollectionByName(string collectionName = "")
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);

            return collection;
        }

        private string GetCollectionNameForToday()
        {
            if (!string.IsNullOrEmpty(todayCollectionNameCached))
            {
                var dateTimeNow = DateTime.Now;

                if (Regex.IsMatch(todayCollectionNameCached, @"^.+_" + $"{dateTimeNow.Year}{dateTimeNow.Month}{dateTimeNow.Day}" + @"$"))
                {
                    return todayCollectionNameCached;
                }
            }

            string todayDateFormatted = DateTime.Now.ToString("yyyyMMdd");
            string todayCollectionName = string.Concat(_dbName, '_', todayDateFormatted);

            return todayCollectionName;
        }
    }
}
