using Client.Models;
using Client.Dto;

namespace Client.Dao
{
    public interface ITourLogDao : IDao<TourLog>
    {
        public Task<IEnumerable<TourLog>> ReadMultiple(int tourid);
    };

    public class TourLogDao : HttpDao<TourLog>, ITourLogDao
    {

        public TourLogDao(IHttpService http) : base(http) { }
        public override async Task Create(TourLog tourLog)
        {
            await http.Post(tourLog.ToCreateTourLogDto(), $"Tours/{tourLog.Tour.Id}/logs");
        }
        public override async Task<TourLog> Read(int tourLogId)
        {
            return await http.Get<TourLog>($"Tours/logs/{tourLogId}");
        }

        public async Task<IEnumerable<TourLog>> ReadMultiple(int tourid)
        {
            return await http.Get<IEnumerable<TourLog>>($"Tours/{tourid}/logs");
        }
        public override Task<IEnumerable<TourLog>> ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> Update(TourLog tourLog)
        {
            var dto = tourLog.ToUpdateTourLogDto();
            return await http.Put(dto, $"Tours/logs/{dto!.Id}"); 
        }

        public override async Task Delete(TourLog tourLog)
        {
            await http.Delete($"Tours/logs/{tourLog!.Id}");
        }
    }
}