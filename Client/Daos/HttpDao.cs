namespace Client.Dao;

public abstract class HttpDao<TDto> : Dao<TDto> where TDto : class
{
    protected IHttpService http;
    protected TDto? model;
    public HttpDao(IHttpService http)
    {
        this.http = http;
    }
}