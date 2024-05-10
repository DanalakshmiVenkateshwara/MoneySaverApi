using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Linq;
using static Dapper.SqlMapper;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RepositoryBase : IRepository
    {
        private IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// pulling list of data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<T>> All<T>(string sqlQuery, object param = null)
        {
            var result = (await _unitOfWork.Connection.QueryAsync<T>(DecorateTransaction(sqlQuery), param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text)).ToList();
            return result;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public async Task<T> Find<T>(string sqlQuery)
        {
            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<T>(DecorateTransaction(sqlQuery), transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// pulling the FirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Find<T>(string sqlQuery, object parameter)
        {
            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<T>(DecorateTransaction(sqlQuery), parameter, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// pulling the FirstOrDefault
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        public async Task<T> FindBy<T>(string sqlQuery, object entityParam)
        {
            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<T>(DecorateTransaction(sqlQuery), entityParam, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// pulling the FirstOrDefault
        /// </summary>
        /// <param name="uspName"></param>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        public async Task<object> FindBy(string sqlQuery, object entityParam)
        {
            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<object>(DecorateTransaction(sqlQuery), entityParam, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// Delete multiple records
        /// </summary>
        /// <param name="uspName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteMultiple(string sqlQuery, string ids)
        {
            await _unitOfWork.Connection.ExecuteAsync(DecorateTransaction(sqlQuery), new { Id = ids }, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
        }

        /// <summary>
        /// Delete multiple records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteMultiple<T>(string sqlQuery, T entity)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(entity);
            dynamicParameters.Add("@res", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _unitOfWork.Connection.ExecuteAsync(DecorateTransaction(sqlQuery), dynamicParameters, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return dynamicParameters.Get<int>("@res");
        }

        /// <summary>
        ///  Delete a record
        /// </summary>
        /// <param name="uspName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(string sqlQuery)
        {
            var result = await _unitOfWork.Connection.ExecuteAsync(DecorateTransaction(sqlQuery), transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        ///  Delete a record
        /// </summary>
        /// <param name="uspName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Delete(string sqlQuery, int id)
        {
            var result = await _unitOfWork.Connection.ExecuteAsync(DecorateTransaction(sqlQuery), new { Id = id }, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            return result;
        }

        /// <summary>
        /// Finding the record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<T> FindByName<T>(string name)
        {
            var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<T>(DecorateTransaction("SELECT * FROM T WHERE Name = @Name"), param: new { Name = name });
            return result;
        }


        /// <summary>
        /// create or update records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="entity"></param>
        /// <param name="isInsert"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateDynamic<T>(string sqlQuery, T entity, bool isOutParameterRequired = false)
        {
            int result;

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(entity);

            if (isOutParameterRequired)
            {
                dynamicParameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            }

            if (_unitOfWork.Connection.State == ConnectionState.Closed)
                _unitOfWork.Connection.Open();

            if (_unitOfWork.Transaction != null)
                result = await _unitOfWork.Connection.ExecuteAsync(sqlQuery, dynamicParameters, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            else
                result = await _unitOfWork.Connection.ExecuteAsync(sqlQuery, dynamicParameters, commandType: CommandType.Text);

            if (isOutParameterRequired)
            {
                result = dynamicParameters.Get<int>("Id");
            }

            if (_unitOfWork.Transaction == null && _unitOfWork.Connection.State == ConnectionState.Open)
                _unitOfWork.Connection.Close();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        //public async Task BulkSave(DataTable item, string[] param)
        //{
        //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(string.Empty, SqlBulkCopyOptions.KeepIdentity))
        //    {
        //        bulkCopy.BatchSize = 100;
        //        bulkCopy.DestinationTableName = item.TableName;
        //        await bulkCopy.WriteToServerAsync(item);
        //    };
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        //public async Task BulkSaveWithNewIdentity(DataTable item, string[] param)
        //{
        //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(string.Empty))
        //    {
        //        bulkCopy.BatchSize = 100;
        //        bulkCopy.DestinationTableName = item.TableName;
        //        await bulkCopy.WriteToServerAsync(item);
        //    };
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //public async Task<T> ValidateUser<T>(string sqlQuery, object entityParam)
        //{
        //    var result = (await _unitOfWork.OptimizConnection.QueryFirstOrDefaultAsync<T>(DecorateTransaction(sqlQuery), entityParam));
        //    return result;
        //}

        /// <summary>
        /// pulling multiple data objects
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="uspName"></param>
        /// <param name="param"></param>
        /// <param name="resultsCount"></param>
        /// <returns></returns>
        public async Task<Tuple<List<T1>, List<T2>>> QueryMultiple<T1, T2>(string sqlQuery, object param, int resultsCount)
        {
            GridReader reader = await _unitOfWork.Connection.QueryMultipleAsync(DecorateTransaction(sqlQuery), param, transaction: _unitOfWork.Transaction, commandType: CommandType.Text);
            List<T1> T1List = resultsCount > 0 ? reader.Read<T1>().ToList() : new List<T1>();
            List<T2> T2List = resultsCount > 1 ? reader.Read<T2>().ToList() : new List<T2>();
            return new Tuple<List<T1>, List<T2>>(T1List, T2List);
        }

        private string DecorateTransaction(string sql)
        {
            if (_unitOfWork.Transaction == null)
                sql = $"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED\r\n{sql}";

            return sql;
        }
    }
}

