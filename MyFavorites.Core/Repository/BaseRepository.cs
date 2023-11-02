using Microsoft.Extensions.Options;
using MyFavorites.Core.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace MyFavorites.Core.Repository
{
    public class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(IOptionsSnapshot<DataBaseSettings> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            using var conn = CreateConnection(_connectionString);
            return await conn.ExecuteScalarAsync<T>(sql, param);
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var conn = CreateConnection(_connectionString);
            return await conn.ExecuteAsync(sql, param);
        }

        protected async Task<int> ExecuteAsync(string sql1, string sql2, object param1 = null, object param2 = null)
        {
            int result = 0;
            using var conn = CreateConnection(_connectionString);
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    result += await ExecuteAsync(sql1, param1);
                    result += await ExecuteAsync(sql2, param2);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    //Console.WriteLine($"事务发生错误: {ex.Message}");
                }
            }
            return result;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            using var conn = CreateConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using var conn = CreateConnection(_connectionString);
            return await conn.QueryAsync<T>(sql, param);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(string strConn)
        {
            if (string.IsNullOrWhiteSpace(strConn))
                throw new ArgumentNullException("获取数据库连接不能为空");
            IDbConnection connection = new MySqlConnection(strConn);
            return connection;
        }
    }
}