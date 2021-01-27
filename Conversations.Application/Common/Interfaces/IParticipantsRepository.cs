using System.Threading.Tasks;
using Conversations.Domain.Entities;

namespace Conversations.Application.Common.Interfaces
{
    public interface IParticipantsRepository
    {
        public Task CreateParticipant(Participant participant);
    }
}