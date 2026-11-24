using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ControlGastos
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Gasto>().Wait();
        }

        // CREATE
        public async Task<int> AddGastoAsync(Gasto gasto) =>
            await _database.InsertAsync(gasto);

        // READ
        public async Task<List<Gasto>> GetGastosAsync() =>
            await _database.Table<Gasto>().ToListAsync();

        // UPDATE
        public async Task<int> UpdateGastoAsync(Gasto gasto) =>
            await _database.UpdateAsync(gasto);

        // DELETE
        public async Task<int> DeleteGastoAsync(Gasto gasto) =>
            await _database.DeleteAsync(gasto);
    }
}
