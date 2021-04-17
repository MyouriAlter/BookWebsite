using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Book.DataAccess.Data;
using Book.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Book.DataAccess.Repository
{
    public class SpCall : ISpCall
    {
        private readonly ApplicationDbContext _dbContext;
        private static string _connectionString = "";


        public SpCall(ApplicationDbContext dbbContext)
        {
            _dbContext = dbbContext;
            _connectionString = dbbContext.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            {
                sqlConnection.Open();
                return (T)Convert.ChangeType(sqlConnection.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure), typeof(T));
            }
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            {
                sqlConnection.Open();
                sqlConnection.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            {
                sqlConnection.Open();
                var value = sqlConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
                return (T) Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            {
                sqlConnection.Open();
                return sqlConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            {
                sqlConnection.Open();
                var result = sqlConnection.QueryMultiple(procedureName, param,
                    commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }
            } 
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }
    }
}
