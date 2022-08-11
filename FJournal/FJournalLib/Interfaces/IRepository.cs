using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalLib.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IMongoCollection<T> GetCollectionByName(string collectionName = "");

        IEnumerable<T> GetRecordsByAmount(int amountOfRecordsToGet);

        T GetRecord(ObjectId id, string collectionName = "");

        void Create(T record);

        void Update(T record);

        void Delete(ObjectId id, string collectionName = "");

        void Save();
    }
}
