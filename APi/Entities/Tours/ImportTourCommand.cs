// using Tourplanner;
// using Tourplanner.DTOs;
// using Tourplanner.Infrastructure;
// using Tourplanner.Repositories;
//
// namespace Tourplanner.Entities.Tours.Commands
// {
//
//     public record ImportTourCommand(CreateTourDto Tour) : IRequest;
//
//
//     public class ImportTourCommandHandler(
//         TourContext ctx,
//         ITourRepository tourRepository) : RequestHandler<ImportTourCommand, int>(ctx)
//     {
//         public override async Task<int> Handle(ImportTourCommand command)
//         {
//             var createTourCommand = new CreateTourCommand(
//                 command.Tour.Name,
//                 command.Tour.Description,
//                 command.Tour.From,
//                 command.Tour.To,
//                 command.Tour.TransportType,
//                 command.Tour.Start,
//                 command.Tour.Destination);
//             
//             
//         }
//
//
//     }
// }