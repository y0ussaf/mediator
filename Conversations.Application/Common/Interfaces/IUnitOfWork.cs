using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Conversations.Application.Common.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task BeginWork();
        Task CommitWork();
        Task RollBack();
    }
}