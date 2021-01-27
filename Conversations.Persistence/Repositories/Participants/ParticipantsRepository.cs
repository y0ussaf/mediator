using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using Dapper;

namespace Conversations.Persistence.Repositories.Participants
{
    public class ParticipantsRepository : IParticipantsRepository
    {
        private readonly IUnitOfWorkContext _unitOfWork;

        public ParticipantsRepository(IUnitOfWorkContext unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateParticipant(Participant participant)
        {
            var connection = _unitOfWork.GetSqlConnection(); 
            await connection.ExecuteAsync(
                @"insert into Participants values (@name,@pseudoName)"
                ,
                new
                {
                    name = participant.Name,
                    pseudoName = participant.PseudoName
                },
                _unitOfWork.GetTransaction()
            );
        }
    }
}