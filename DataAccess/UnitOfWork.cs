using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Assigning local pickup and delivery connection object
        /// </summary>
        public IDbConnection Connection => _connection ?? (_connection = _connectionFactory.Connection);

        /// <summary>
        /// Assigning optimiz connection object
        /// </summary>
        //public IDbConnection OptimizConnection => _optimizConnection ?? (_optimizConnection = _connectionFactory.OptimizConnection);

        public IDbTransaction Transaction { get; set; } = null;

        /// <summary>
        /// Transaction begin
        /// </summary>
        public void Begin()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
                Transaction = Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// Transaction commint
        /// </summary>
        public void Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;

            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Close();
        }

        /// <summary>
        /// Transaction rollback
        /// </summary>
        public void Rollback()
        {
            Transaction?.Rollback();

            Transaction?.Dispose();
            Transaction = null;

            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Close();

        }
    }
}

