using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Dapper;

namespace Conversations.Persistence.Repositories.Conversations
{
    public class ConversationsRepository : IConversationsRepository
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;

        public ConversationsRepository(IUnitOfWorkContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }
        
        public async Task<Conversation> GetConversationById(int conversationId)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var conversation = await connection.QuerySingleOrDefaultAsync<Conversation>(
                @"select * from Conversations  where Id = @id", new
            {
                id = conversationId
            },_unitOfWorkContext.GetTransaction());
            return conversation;
        }

        public async Task<List<Participant>> GetConversationParticipants(int conversationId)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var result = await connection.QueryAsync<Participant>(
                @"select * from ConversationsParticipants as CP inner join participants as P
                    on CP.ParticipantId = P.Id where CP.ConversationId = @id"
                , 
                new
                {
                    id = conversationId
                },_unitOfWorkContext.GetTransaction());
            return result.ToList();
        }

        public async Task<bool> ParticipantBelongsToConversation(int conversationId,int participantId)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var result  = await connection.QuerySingleOrDefaultAsync<int?>(
                @"select 1 from ConversationsParticipants as CP where 
                CP.ConversationId = @conversationId and CP.ParticipantId = @participantId"
                ,
                new
                {
                    conversationId,
                    participantId
                },
                _unitOfWorkContext.GetTransaction()
            );

            return result.HasValue;
        }

        public async Task<List<Message>> GetConversationMessages(int conversationId,int page, int pageSize)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var result = await connection.QueryAsync<Message,Participant,Message>(
                @"select * from Messages as M inner join Participants as P on M.AuthorId = P.Id where M.ConversationId = @conversationId
                order by CreatedAt offset @offset rows fetch next @nbrOfRows rows only"
                ,(message, participant) =>
                {
                    message.Author = participant;
                    return message;
                },
                new
                {
                    conversationId,
                    offset = page*pageSize,
                    nbrOfRows = pageSize
                },
                _unitOfWorkContext.GetTransaction()
            );

            return result.ToList();
        }

        public async Task<List<Message>> LatestMessagesInEachConversation(int participantId,int page, int pageSize)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var result = await connection.QueryAsync<Message,Participant,Message>(
                @"select M1.*,P.* from ConversationsParticipants as CP inner join Messages as M1
                on M1.ConversationId = CP.ConversationId
                inner join Participants P on M1.AuthorId = P.Id
                where CP.ParticipantId = @participantId 
                and  M1.CreatedAt = (select MAX(M2.CreatedAt) from messages as M2 where M2.ConversationId = CP.ConversationId)
                order by M1.CreatedAt DESC offset @offset rows fetch next @nbrOfRows rows only 
                 ",(message, participant) =>
                {
                    message.Author = participant;
                    return message;
                },new
                {
                    participantId,
                    offset = page*pageSize,
                    nbrOfRows = pageSize
                },
                _unitOfWorkContext.GetTransaction()
            );
            return result.ToList();
        }

        public async Task CreateConversation(Conversation conversation)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            var dt = new DataTable();
            dt.Columns.Add("ParticipantId",typeof(int));
            dt.Columns.Add("Role",typeof(byte?));
            foreach (var conversationParticipant in conversation.ConversationParticipants)
            {
                dt.Rows.Add(conversationParticipant.Participant.Id,(byte?) conversationParticipant.Role);
            }
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@CreatedAt",conversation.CreatedAt, DbType.DateTime2);
            dynamicParameters.Add("@Type",(int) conversation.Type);
            dynamicParameters.Add("@ConversationId",dbType:DbType.Int32,direction: ParameterDirection.Output);
            dynamicParameters.Add("@ParticipantsWithRole",dt.AsTableValuedParameter("ParticipantsWithRole"));
            await connection.ExecuteAsync(
                @"St_InsertConversation",dynamicParameters,_unitOfWorkContext.GetTransaction(),commandType:CommandType.StoredProcedure
            );
            var conversationId = dynamicParameters.Get<int>("@ConversationId");
            conversation.Id = conversationId;

        }

        public async Task AddParticipantToConversation(int conversationId, int participantId)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            await connection.ExecuteAsync(
                @"insert into ConversationsParticipants (ParticipantId, ConversationId, CreatedAt) values (@participantId,@conversationId,@createdAt);"
                ,new
                {
                    participantId,
                    conversationId,
                    createdAt = DateTime.Now
                },
                _unitOfWorkContext.GetTransaction()
                );
        }

        public async Task DeleteParticipantFromConversation(int conversationId, int participantId)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            await connection.ExecuteAsync(
                @"delete from ConversationsParticipants where ConversationId = @conversationId and ParticipantId = @participantId"
                , new
                {
                    conversationId,
                    participantId
                },
                _unitOfWorkContext.GetTransaction()
            );
        }

        public async Task AddMessage(int conversationId, int authorId, Message message)
        {
            var connection = _unitOfWorkContext.GetSqlConnection();
            await connection.ExecuteAsync(
                @"insert into Messages values (@content,@createdAt,@conversationId,@authorId)",
                new
                {
                    content = message.Content,
                    createdAt = message.CreatedAt,
                    conversationId,
                    authorId
                },
                _unitOfWorkContext.GetTransaction()
            );
        }
    }
}