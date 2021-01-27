using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conversations.Application.Common.Interfaces
{
    public interface IConversationsRepository
    {

        public Task<Conversation> GetConversationById(int conversationId);
        public Task<List<Participant>> GetConversationParticipants(int conversationId);
        public Task<bool> ParticipantBelongsToConversation(int conversationId,int participantId);
        public Task<List<Message>> GetConversationMessages(int conversationId,int page,int pageSize);
        public Task<List<Message>> LatestMessagesInEachConversation(int participantId,int page, int pageSize);
        public Task CreateConversation(Conversation conversation);
        public Task AddParticipantToConversation(int conversationId, int participantId);
        public Task DeleteParticipantFromConversation(int conversationId, int participantId);
        public Task AddMessage(int conversationId, int authorId,Message message);
        
    }
}