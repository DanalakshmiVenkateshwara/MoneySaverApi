
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.Common;
//using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionStrings _connectionStrings = null;
        private readonly DbProviderFactory _dataProvideFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

        /// <summary>
        /// getting the local pickup and delivery database connection 
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                IDbConnection connection = EstablishPandDConnection();

                if (connection.State.Equals(ConnectionState.Closed))
                {
                    if (string.IsNullOrEmpty(connection.ConnectionString))
                    {
                        connection.ConnectionString = _connectionStrings.SampleConnectionString;
                    }
                }

                return connection;
            }
        }

        private IDbConnection EstablishPandDConnection()
        {
            var conn = _dataProvideFactory.CreateConnection();
            conn.ConnectionString = _connectionStrings.SampleConnectionString;
            return conn;
        }

        /// <summary>
        /// setting the database options
        /// </summary>
        /// <param name="databaseOptions"></param>
        public ConnectionFactory(IOptions<ConnectionStrings> options)
        {
            _connectionStrings = options.Value;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
