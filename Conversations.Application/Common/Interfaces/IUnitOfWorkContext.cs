using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Conversations.Application.Common.Interfaces
{
    public interface IUnitOfWorkContext
    {
        public DbTransaction GetTransaction();
        public SqlConnection GetSqlConnection();
        Task<IUnitOfWork> CreateUnitOfWork();
    }
}