using Client.Models;
using Client.Dto;

namespace Client.Dao
{
    public interface ITourLogDao<TourLog> : IDao<TourLog> where TourLog : class { }

    public class TourLogDao : HttpDao<TourLog>, ITourLogDao<TourLog>
    {

        public TourLogDao(IHttpService http) : base(http) { }
        public override async Task Create()
        {
            await http.Post<TourLog>(this.model!, $"TourLogs");
        }
        public override async Task<TourLog> Read(TourLog _model)
        {
            return await http.Get<TourLog>($"Tours/{_model.Id}");
        }

        public async Task<IEnumerable<TourLog>> ReadMultiple(int tourid)
        {
            return await http.Get<IEnumerable<TourLog>>($"Tours/{tourid}/logs");
        }

        public override Task<IEnumerable<TourLog>> ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> Update()
        {

            UpdateTourLogDto updateTourLogDto = new UpdateTourLogDto(
                 model!.Id,
                 model!.Comment,
                 model!.Difficulty,
                 model!.Duration,
                 model!.Rating
            );

            return await http.Put<UpdateTourLogDto>(updateTourLogDto, $"logs/{updateTourLogDto!.Id}"); 
        }

        public override async Task Delete()
        {
            await http.Delete($"TourLogs/{model!.Id}");
        }
    }
}