using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppMCQs.Models
{
    public class QuestionsDatabaseform
    {
        SQLiteAsyncConnection Database;
        private SQLiteAsyncConnection _dbConnection;
        public const string DatabaseFilename = "MCQform.db3";
        async Task Init()
        {
            string dbPath =
             Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
            FileStream fileStream = new FileStream(dbPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            _dbConnection = new SQLiteAsyncConnection(dbPath);
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(dbPath, Constants.Flags);

            var result = await Database.CreateTableAsync<QuestionF>();
        }
        public async Task<List<QuestionF>> GetItemsAsync()
        {
            await Init();
            var res = await Database.Table<QuestionF>().ToListAsync();
            return res;
        }
        public async Task<QuestionF> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<QuestionF>().Where(i => i.No == id).FirstOrDefaultAsync();
        }


        public async Task<int> SaveItemAsync(QuestionF item)
        {
            await Init();
            QuestionF res = await GetItemAsync(item.No);
            if (res != null)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }


        public async Task<int> DeleteItemAsync(QuestionF item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }



    }
}
