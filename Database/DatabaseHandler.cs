using HealthApp.Database.Tables;
using SQLite;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using static SQLite.TableMapping;

namespace HealthApp.Database
{
    public class DatabaseHandler
    {
        private const string DB_Name = "UserData.db";
        private readonly SQLiteAsyncConnection _connection;

        public DatabaseHandler()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));

            _connection.CreateTableAsync<User>();

            _connection.CreateTableAsync<Medicines>();

        }

        public async Task<List<Medicines>> GetMedicines()
        {
            return await _connection.Table<Medicines>().ToListAsync();
        }

        public async Task<Medicines> GetById(int id)
        {
            return await _connection.Table<Medicines>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddMed(Medicines medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                await _connection.InsertAsync(medicine);
            }
        }

        public async Task UpdateMed(Medicines medicine)
        {
            await _connection.UpdateAsync(medicine);
        }
    }
}
