
namespace Conversations.Application.Queries.Conversations.Messages.GetLatestMessageInEachConversation
{
    public class MessageDto
    {
        public string Content { get; set; }
        public ParticipantDto Author { get; set; }
    }
}