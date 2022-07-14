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

namespace FJournalLib.Repositories
{
    internal class MongoRecordRepository : IRepository<DBRecord>
    {
        private static readonly string _dbName =                "FJournalDb";
        private static readonly string _connectionString =      "mongodb://localhost:27017/FJounalDb";

        private readonly IMongoDatabase _database;

        private string todayCollectionNameCached = string.Empty;

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

        public void Delete(uint id, string collectionName = "")
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);
            collection.DeleteOne(x => x.Id == id);
        }

        public void Dispose()
        {

        }

        public DBRecord GetRecord(uint id, string collectionName = "")
        {
            string localCollectionName = string.IsNullOrEmpty(collectionName) ? this.GetCollectionNameForToday() : collectionName;

            var collection = this._database.GetCollection<DBRecord>(localCollectionName);
            var record = collection.AsQueryable()
                                    .SingleOrDefault(x => x.Id == id);

            return record;
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
            if (!string.IsNullOrEmpty(this.todayCollectionNameCached))
            {
                var dateTimeNow = DateTime.Now;

                if (Regex.IsMatch(this.todayCollectionNameCached, @"^.+_" + $"{dateTimeNow.Year}{dateTimeNow.Month}{dateTimeNow.Day}" + @"$"))
                {
                    return this.todayCollectionNameCached;
                }
            }

            string todayDateFormatted = DateTime.Now.ToString("yyyyMMdd");
            string todayCollectionName = string.Concat(_dbName, '_', todayDateFormatted);

            return todayCollectionName;
        }
    }
}
