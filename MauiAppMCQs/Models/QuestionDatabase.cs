using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
 
namespace MauiAppMCQs.Models
{
    public class QuestionsDatabase
    {
        SQLiteAsyncConnection Database;
        private SQLiteAsyncConnection _dbConnection;
        public const string DatabaseFilename = "MCQs.db3";
        async Task Init()
        {  
        string dbPath  =
         Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
            _dbConnection = new SQLiteAsyncConnection(dbPath);
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(dbPath, Constants.Flags);
            var result = await Database.CreateTableAsync<InQuestion>();
        }
        public async Task<List<InQuestion>> GetItemsAsync()
        {
            await Init();
            var res = await Database.Table<InQuestion>().ToListAsync();
            return res;
        }
        public async Task<InQuestion> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<InQuestion>().Where(i => i.No == id).FirstOrDefaultAsync();
        }


        public async Task<int> SaveItemAsync(InQuestion item)
        {
            await Init();
            InQuestion res = await GetItemAsync(item.No);
            if (res != null)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }


        public async Task<int> DeleteItemAsync(InQuestion item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
