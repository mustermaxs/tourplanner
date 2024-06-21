using Tourplanner;
using Tourplanner.Db;
using Tourplanner.DTOs;
using Tourplanner.Entities;
using Tourplanner.Entities.Tours;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;
using Tourplanner.Models;
using Tourplanner.Repositories;

namespace Api.Entities.Maps
{
    public record DeleteMapForTourCommand(int TourId) : IRequest;

    public class DeleteMapForTourCommandHandler(
        IUnitOfWork unitOfWork) : RequestHandler<DeleteMapForTourCommand, Task>()
    {
        public override async Task<Task> Handle(DeleteMapForTourCommand request)
        {
            unitOfWork.BeginTransactionAsync();
            try
            {
                var map = await unitOfWork.MapRepository.GetMapByTourId(request.TourId);
                if (map is null) throw new ResourceNotFoundException("Map not found");

                await unitOfWork.MapRepository.Delete(map);
                await unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}