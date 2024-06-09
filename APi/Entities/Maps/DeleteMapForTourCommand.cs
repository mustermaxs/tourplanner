using Tourplanner;
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
        TourContext ctx,
        IMapRepository mapRepository,
        ITileRepository tileRepository) : RequestHandler<DeleteMapForTourCommand, Task>(ctx)
    {
        public override async Task<Task> Handle(DeleteMapForTourCommand request)
        {
            var map = await mapRepository.GetMapByTourId(request.TourId);
            if (map is null) throw new ResourceNotFoundException("Map not found");

            await mapRepository.Delete(map);
            
            return Task.CompletedTask;
        }
    }
}