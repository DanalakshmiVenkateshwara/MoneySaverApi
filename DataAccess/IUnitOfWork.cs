using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; set; }
        void Begin();
        void Commit();
        void Rollback();
    }
}
