using System;
using System.Threading;
using System.Threading.Tasks;
using Conversations.Application.Common.Interfaces;
using Conversations.Domain.Entities;
using MediatR;

namespace Conversations.Application.Commands.Participants.CreateParticipant
{
    public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommand>
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly IParticipantsRepository _participantsRepository;

        public CreateParticipantCommandHandler(IUnitOfWorkContext unitOfWorkContext,IParticipantsRepository participantsRepository)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _participantsRepository = participantsRepository;
        }

        public async Task<Unit> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
        {
            await using (var unitOfWork = await _unitOfWorkContext.CreateUnitOfWork())
            {
                await unitOfWork.BeginWork();
                try
                {
                    await _participantsRepository.CreateParticipant(new Participant
                    {
                        Id = request.ParticipantId,
                        Name = request.Name
                    });
                    await unitOfWork.CommitWork();
                }
                catch (Exception)
                {
                    await unitOfWork.RollBack();
                    throw;
                }
            }
            return Unit.Value;
        }
    }
}