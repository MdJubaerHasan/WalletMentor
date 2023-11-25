using System;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Windows;

namespace WalletMentor
{
    public class MongodbService
    {
        private readonly IMongoDatabase _database;
        

        public MongodbService()
        {
            var connectionString = "mongodb+srv://pesos8:pesos8@vault10.bpchsp0.mongodb.net/?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("WalletSummary");
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public async Task InsertOrUpdateField(string month, int year, double amount, string fieldName , IMongoCollection<MonthlyReport> _collection)
        {
            var filter = Builders<MonthlyReport>.Filter.Eq(r => r.Month, month) & Builders<MonthlyReport>.Filter.Eq(r => r.Year, year);
            var update = Builders<MonthlyReport>.Update.Set(fieldName, amount);
            var options = new UpdateOptions { IsUpsert = true };
            await _collection.UpdateOneAsync(filter, update, options);
        }

    }
}
