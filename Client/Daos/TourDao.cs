using Client.Models;

namespace Client.Dao
{
    public interface ITourDao : IDao<Tour>
    {
    }

    public class TourDao : HttpDao<Tour>, ITourDao
    {
        public TourDao(IHttpService http) : base(http)
        {
        }

        public override async Task Create(Tour tour)
        {
            var createTourDto = new CreateTourDto(
                name: tour.Name,
                description: tour.Description,
                from: tour.From,
                to: tour.To,
                distance: tour.Distance,
                estimatedTime: tour.EstimatedTime,
                transportType: tour.TransportType
            );
            
            await http.Post<CreateTourDto>(createTourDto, $"Tours");
        }

        public override async Task<Tour> Read(Tour tourLog)
        {
            return await http.Get<Tour>($"Tours/{tourLog.Id}");
        }

        public override async Task<IEnumerable<Tour>> ReadMultiple()
        {
            return await http.Get<IEnumerable<Tour>>("Tours");
        }

        public override async Task Update(Tour tour)
        {
            var updateTourDto = new UpdateTourDto(
                tour.Name,
                tour.Description,
                tour.From,
                tour.To,
                tour.TransportType
            );
            await http.Put<UpdateTourDto>(updateTourDto, $"Tours/{tour.Id}");
        }

        public override async Task Delete(Tour tour)
        {
            await http.Delete($"Tours/{tour.Id}");
        }
    }
}