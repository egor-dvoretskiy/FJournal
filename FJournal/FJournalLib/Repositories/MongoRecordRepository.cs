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
using FJournalLib.Extensions;
using System.Diagnostics;

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

            var collection = this.GetCollectionByName(localCollectionName);
            var record = collection.AsQueryable()
                                    .SingleOrDefault(x => x.Id == id);

            return record;
        }

        public IEnumerable<DBRecord> GetRecordsByDateCollection(List<DateTime> collection, int amountOfRecordsToGet)
        {
            var collectionNames = this._database.ListCollectionNames().ToList();
            List<string> filteredCollectionNames = new List<string>();

            foreach (var collectionName in collectionNames)
            {
                var amountOfDaysInCollection = collection.Where(x => collectionName.Contains(x.ToString("yyyyMMdd"))).Count();

                if (amountOfDaysInCollection > 0)
                {
                    filteredCollectionNames.Add(collectionName);
                }
            }

            filteredCollectionNames.Reverse();
            
            return GetRecordsFromCollection(filteredCollectionNames, amountOfRecordsToGet);
        }

        public IEnumerable<DBRecord> GetRecordsByAmount(int amountOfRecordsToGet)
        {
            IEnumerable<DBRecord> records;

            var collectionNames = this._database.ListCollectionNames().ToList();
            collectionNames.Reverse();

            records = this.GetRecordsFromCollection(collectionNames, amountOfRecordsToGet);

            return records;
        }

        public void Save()
        {

        }

        public void Update(DBRecord record)
        {
            var collection = this._database.GetCollection<DBRecord>(this.GetCollectionNameForToday());
            collection.ReplaceOne(x => x.Id == record.Id, record);
        }

        public IMongoCollection<DBRecord> GetCollectionByName(string collectionName = "") => this.Memoized(collectionName, x =>
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);

            return collection;
        });

        private string GetCollectionNameForToday()
        {
            if (!string.IsNullOrEmpty(todayCollectionNameCached))
            {
                if (todayCollectionNameCached.IsToday())
                {
                    return todayCollectionNameCached;
                }
            }

            string todayCollectionName = _dbName.FormTodayCollectionName();
            todayCollectionNameCached = todayCollectionName;

            return todayCollectionName;
        }

        private IEnumerable<DBRecord> GetRecordsFromCollection(List<string> collectionNames, int amountOfRecordsToGet) => this.Memoized((string.Join(".", collectionNames), amountOfRecordsToGet), x =>
        {
            List<DBRecord> records = new List<DBRecord>();

            foreach (var collectionName in collectionNames)
            {
                var collection = this.GetCollectionByName(collectionName);

                if (records.Count < amountOfRecordsToGet)
                {
                    int amountOfRecordsToFill = Math.Abs(amountOfRecordsToGet - records.Count);
                    var amountOfItemsInCollection = collection.EstimatedDocumentCount();

                    try
                    {
                        if (amountOfRecordsToFill >= amountOfItemsInCollection)
                        {
                            records.AddRange(collection.Find(new BsonDocument()).ToEnumerable());
                        }
                        else
                        {
                            int skipAmount = (int)(amountOfItemsInCollection - amountOfRecordsToFill);

                            return collection.Find(new BsonDocument()).Skip(skipAmount).ToEnumerable().OrderBy(x => x.TimeStamp);
                        }
                    }
                    catch (FormatException) { }
                }
            }

            var orderedRecordsByTimeStamp = records.OrderBy(x => x.TimeStamp);

            return orderedRecordsByTimeStamp;
        });
    }
}
