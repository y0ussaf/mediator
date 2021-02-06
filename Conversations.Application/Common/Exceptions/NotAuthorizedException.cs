using System;

namespace Conversations.Application.Common.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
        {
            
        }
        public NotAuthorizedException(string message) : base(message)
        {
        }
    }
}