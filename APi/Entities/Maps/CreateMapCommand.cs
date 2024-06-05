// namespace Tourplanner.Entities.Maps
// {
//     using Tourplanner.Infrastructure;
//     using Tourplanner.Services;

//     public record CreateMapCommand(Tour Tour) : IRequest;

//     public class CreateMapCommandHandler(
//         TourContext ctx,
//         IMapRepository mapRepository,
//         ITileRepository tileRepository,
//         ITileService tileService) : RequestHandler<CreateMapCommand, Task>(ctx);
//     {
//         public override async Task Handle(CreateMapCommand cmd)
//         {
//             var tileConfigs = 
//         }
//     }

// }