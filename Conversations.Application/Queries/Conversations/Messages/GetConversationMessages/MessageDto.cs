
namespace Conversations.Application.Queries.Conversations.Messages.GetConversationMessages
{
    public class MessageDto
    {
        public ParticipantDto Author { get; set; }
        public string Content { get; set; }
    }
}