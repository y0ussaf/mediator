using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;

namespace Conversations.Persistence
{
    public class UnitOfWorkContext : IUnitOfWorkContext
    {
        private readonly SqlConnection _sqlConnection;
        private UnitOfWork _unitOfWork;

        public UnitOfWorkContext(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<IUnitOfWork> CreateUnitOfWork()
        {
            if (_sqlConnection.State != ConnectionState.Open)
            {
                await _sqlConnection.OpenAsync();
            }
            if (_unitOfWork!= null && !_unitOfWork.IsDisposed)
            {
                throw new Exception("the current unit of work should be disposed before creating new one");
            }
            _unitOfWork =  new UnitOfWork(_sqlConnection);
            return _unitOfWork;
        }

        public DbTransaction GetTransaction()
        {
            if (_unitOfWork?.Transaction is null)
            {
                throw new Exception("create unit of work if it's not already created and begin new transaction");
            }
            return _unitOfWork.Transaction;
        }

        public SqlConnection GetSqlConnection()
        {
            if (_unitOfWork is null || _unitOfWork.IsDisposed || (_unitOfWork.Transaction is null) )
            {
                throw new Exception(@"create unit of work if it's not already created and begin new transaction begin new transaction 
                                            before trying to access sql connection");
            }
            return _sqlConnection;
        }
        
    }
}