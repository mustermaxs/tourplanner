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
            var createTourLogDto = new CreateTourLogDto(
                tourLog.DateTime,
                tourLog.Comment,
                tourLog.Difficulty,
                tourLog.Duration,
                tourLog.Rating
                );
                
            await http.Post<CreateTourLogDto>(createTourLogDto, $"Tours/{tourLog.Tour.Id}/logs");
        }
        public override async Task<TourLog> Read(TourLog tourLog)
        {
            return await http.Get<TourLog>($"Tours/logs/{tourLog.Id}");
        }

        public async Task<IEnumerable<TourLog>> ReadMultiple(int tourid)
        {
            return await http.Get<IEnumerable<TourLog>>($"Tours/{tourid}/logs");
        }
        public override Task<IEnumerable<TourLog>> ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> Update(TourLog model)
        {

            UpdateTourLogDto updateTourLogDto = new UpdateTourLogDto(
                 model!.Id,
                 model!.Comment,
                 model!.Difficulty,
                 model!.DateTime,
                 model!.Rating
            );

            return await http.Put<UpdateTourLogDto>(updateTourLogDto, $"Tours/logs/{updateTourLogDto!.Id}"); 
        }

        public override async Task Delete(TourLog tourLog)
        {
            await http.Delete($"Tours/logs/{tourLog!.Id}");
        }
    }
}