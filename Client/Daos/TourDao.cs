using Client.Dtos;
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

        public override async Task<HttpResponseMessage> Create(Tour tour)
        {
            return await http.Post(tour.ToCreateTourDto(), $"Tours");
        }

        public override async Task<Tour> Read(int TourId)
        {
            return await http.Get<Tour>($"Tours/{TourId}");
        }

        public override async Task<IEnumerable<Tour>> ReadMultiple()
        {
            return await http.Get<IEnumerable<Tour>>("Tours");
        }

        public override async Task<HttpResponseMessage> Update(Tour tour)
        {
            return await http.Put(tour.ToUpdateTourDto(), $"Tours/{tour.Id}");
        }

        public override async Task Delete(Tour tour)
        {
            await http.Delete($"Tours/{tour.Id}");
        }
    }
}