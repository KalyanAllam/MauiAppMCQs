using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{
    public class QuestionsDatabaseimgs
    {
        SQLiteAsyncConnection Database;
        private SQLiteAsyncConnection _dbConnection;
        public const string DatabaseFilename = "MCQimgs.db3";
        async Task Init()
        {
            string dbPath =
             Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
            FileStream fileStream = new FileStream(dbPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            _dbConnection = new SQLiteAsyncConnection(dbPath);
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(dbPath, Constants.Flags);

            var result = await Database.CreateTableAsync<QuestionI>();
        }
        public async Task<List<QuestionI>> GetItemsAsync()
        {
            await Init();
            var res = await Database.Table<QuestionI>().ToListAsync();
            return res;
        }
        public async Task<QuestionI> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<QuestionI>().Where(i => i.No == id).FirstOrDefaultAsync();
        }


        public async Task<int> SaveItemAsync(QuestionI item)
        {
            await Init();
            QuestionI res = await GetItemAsync(item.No);
            if (res != null)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }


        public async Task<int> DeleteItemAsync(QuestionI item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }



    }
}
