using Client.Models;

namespace Client.Dao;

public interface IMapDao : IDao<Map> {}

public class MapDao : HttpDao<Map>, IMapDao
{
    public MapDao(IHttpService http) : base(http)
    {
    }

    public override Task Create(Map dto)
    {
        throw new NotImplementedException();
    }

    public override Task OnCreate(Map dto)
    {
        throw new NotImplementedException();
    }

    public override async Task<Map> OnRead(int TourId)
    {
        return await http.Get<Map>($"Tours/{TourId}/map");
    }

    public override Task<IEnumerable<Map>> OnReadMultiple()
    {
        throw new NotImplementedException();
    }

    public override Task OnUpdate(Map dto)
    {
        throw new NotImplementedException();
    }

    public override Task OnDelete(Map dto)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<Map>> ReadMultiple()
    {
        throw new NotImplementedException();
    }

    public override Task Update(Map dto)
    {
        throw new NotImplementedException();
    }

    public override Task Delete(Map dto)
    {
        throw new NotImplementedException();
    }
}