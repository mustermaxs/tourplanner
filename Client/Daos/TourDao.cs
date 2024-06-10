using Client.Dtos;
using Client.Exceptions;
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

        public override async Task<HttpResponseMessage> OnCreate(Tour tour)
        {
            return await http.Post(tour.ToCreateTourDto(), $"Tours");
        }

        public override async Task<Tour> OnRead(int TourId)
        {
            return await http.Get<Tour>($"Tours/{TourId}");
        }

        public override async Task<IEnumerable<Tour>> OnReadMultiple()
        {
            return await http.Get<IEnumerable<Tour>>("Toours");
        }

        public override async Task<HttpResponseMessage> OnUpdate(Tour tour)
        {
            return await http.Put(tour.ToUpdateTourDto(), $"Tours/{tour.Id}");
        }

        public override async Task OnDelete(Tour tour)
        {
            await http.Delete($"Tours/{tour.Id}");
        }
    }
}