using System.Linq.Expressions;
using MedLedger.Api.Data.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MedLedger.Api.Data.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : EntityBase
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IOptions<MongoOptions> mongoOptions)
    {
        var client = new MongoClient(mongoOptions.Value.ConnectionString);
        var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name.ToLower() + "s");
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var data =  await _collection.Find(filter).FirstOrDefaultAsync();
        return data;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }
}