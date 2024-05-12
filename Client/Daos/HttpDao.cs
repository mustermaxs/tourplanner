namespace Client.Dao;

public abstract class HttpDao<TDto> : Dao<TDto> where TDto : class
{
    protected IHttpService http;
    protected TDto? model;
    public HttpDao(IHttpService http)
    {
        this.http = http;
    }

    public override abstract Task Create(TDto dto);
    public override abstract Task<TDto> Read(TDto tourLog);
    public override abstract Task<IEnumerable<TDto>> ReadMultiple();
    public override abstract Task Update(TDto dto);
    public override abstract Task Delete(TDto dto);
    public override void SeTDto(TDto model)
    {
        this.model = model;
    }
}