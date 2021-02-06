using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Common.Interfaces
{
    public interface IConversationsRepository
    {

        public Task<Conversation> GetConversationById(string conversationId);
        public Task<List<Participant>> GetConversationParticipants(string conversationId);
        public Task<bool> ParticipantBelongsToConversation(string conversationId,string participantId);
        public Task<List<Message>> GetConversationMessages(string conversationId,int page,int pageSize);
        public Task<List<Message>> LatestMessagesInEachConversation(string participantId,int page, int pageSize);
        public Task CreateConversation(Conversation conversation);
        public Task AddParticipantToConversation(string conversationId, string participantId);
        public Task DeleteParticipantFromConversation(string conversationId, string participantId);
        public Task AddMessage(string conversationId, string authorId,Message message);
        public Task DeleteConversation(string conversationId);

    }
}