using Client.Models;
using Client.Dto;
using Client.Exceptions;

namespace Client.Dao
{
    public interface ITourLogDao : IDao<TourLog>
    {
        public Task<IEnumerable<TourLog>> ReadMultiple(int tourid);
    };

    public class TourLogDao : HttpDao<TourLog>, ITourLogDao
    {
        public TourLogDao(IHttpService http) : base(http)
        {
        }

        public override async Task OnCreate(TourLog tourLog)
        {
            await http.Post(tourLog.ToCreateTourLogDto(), $"Tours/{tourLog.Tour.Id}/logs");
        }

        public override async Task<TourLog> OnRead(int tourLogId)
        {
            return await http.Get<TourLog>($"Tours/logs/{tourLogId}");
        }

        public async Task<IEnumerable<TourLog>> OnReadMultiple(int tourid)
        {
            return await http.Get<IEnumerable<TourLog>>($"Tours/{tourid}/logs");
        }

        public override Task<IEnumerable<TourLog>> OnReadMultiple()
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> OnUpdate(TourLog tourLog)
        {
            var dto = tourLog.ToUpdateTourLogDto();
            return await http.Put(dto, $"Tours/logs/{dto!.Id}");
        }

        public override async Task OnDelete(TourLog tourLog)
        {
            await http.Delete($"Tours/logs/{tourLog!.Id}");
        }

        public async Task<IEnumerable<TourLog>> ReadMultiple(int tourid)
        {
            return await http.Get<IEnumerable<TourLog>>($"Tours/{tourid}/logs");
        }
    }
}