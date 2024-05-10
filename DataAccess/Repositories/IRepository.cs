using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository
    {
        Task<int> AddOrUpdateDynamic<T>(string sqlQuery, T entity, bool isOutParameterRequired = false);
        Task<List<T>> All<T>(string sqlQuery, object param = null);

        Task<int> Delete(string sqlQuery);

        Task<int> Delete(string sqlQuery, int id);

        Task DeleteMultiple(string sqlQuery, string ids);

        Task<int> DeleteMultiple<T>(string sqlQuery, T entity);

        Task<T> Find<T>(string sqlQuery, object parameter);

        Task<T> FindBy<T>(string sqlQuery, object entityParam);

        Task<T> FindByName<T>(string name);


        //Task BulkSave(DataTable item, string[] param);

        //Task BulkSaveWithNewIdentity(DataTable item, string[] param);

        //Task<T> ValidateUser<T>(string sqlQuery, object entityParam);

        Task<Tuple<List<T1>, List<T2>>> QueryMultiple<T1, T2>(string sqlQuery, object param, int resultsCount);
    }
}
