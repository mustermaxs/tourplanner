namespace Client.Dao;

public abstract class HttpDao<TModel> : Dao<TModel> where TModel : class
{
    protected IHttpService http;
    protected TModel? model;
    public HttpDao(IHttpService http)
    {
        this.http = http;
    }

    public override abstract Task Create();
    public override abstract Task<TModel> Read(TModel model);
    public override abstract Task<IEnumerable<TModel>> ReadMultiple();
    public override abstract Task Update();
    public override abstract Task Delete();
    public override void SetModel(TModel model)
    {
        this.model = model;
    }
}