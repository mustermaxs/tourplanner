
using Client.Models;

namespace Client.Dao
{
    public interface ITourDao<Tour> : IDao<Tour> where Tour : class { }

    public class TourDao : HttpDao<Tour>, ITourDao<Tour>
    {

        public TourDao(IHttpService http) : base(http) { }
        public override async Task Create()
        {
            await http.Post<Tour>(this.model!, $"Tours");
        }
        public override async Task<Tour> Read(Tour _model)
        {
            return await http.Get<Tour>($"Tours/{_model.Id}");
        }

        public override Task<IEnumerable<Tour>> ReadMultiple()
        {
            throw new NotImplementedException();
        }

        public override Task Update()
        {
            throw new NotImplementedException();
        }

        public override async Task Delete()
        {
            await http.Delete($"Tours/{model!.Id}");
        }
    }
}
