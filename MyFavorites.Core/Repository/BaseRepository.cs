using Microsoft.Extensions.Options;
using MyFavorites.Core.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using ZstdSharp.Unsafe;

namespace MyFavorites.Core.Repository
{
    public class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(IOptionsSnapshot<DataBaseSettings> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using var conn = CreateConnection(_connectionString);
            return await conn.ExecuteAsync(sql, param);
        }

        protected async Task<int> ExecuteAsync(string sql1, string sql2, object param1 = null, List<MySQLItems> param2 = null)
        {
            var result = 0;
            using var conn = CreateConnection(_connectionString);
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    result += await conn.ExecuteAsync(sql1, param1, transaction);
                    int fid = await conn.QueryFirstAsync<int>("select last_insert_id();", null, transaction);
                    param2.ForEach(item =>
                    {
                        item.Fid ??= fid.ToString();
                    });
                    result += await conn.ExecuteAsync(sql2, param2, transaction);
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