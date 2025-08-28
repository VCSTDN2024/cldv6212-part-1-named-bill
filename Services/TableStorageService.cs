using Azure.Data.Tables;
using azuresolution1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace azuresolution1.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService(string tableName, string connectionString)
        {
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync<T>(T entity) where T : class, ITableEntity
        {
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task<T> GetEntityAsync<T>(string partitionKey, string rowKey) where T : class, ITableEntity
        {
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }

        public async IAsyncEnumerable<T> GetEntitiesAsync<T>() where T : class, ITableEntity
        {
            await foreach (var entity in _tableClient.QueryAsync<T>())
            {
                yield return entity;
            }
        }
    }
}