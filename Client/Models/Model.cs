using Client.Dao;

namespace Client.Models
{

    // public abstract class HttpDao<TModel> : IDao<TModel> where TModel : class
    // {
    //     private IHttpService _httpService;
    //     protected TModel _model;

    //     public HttpDao(IHttpService httpService)
    //     {
    //         this._httpService = httpService;
    //     }

    //     public void SetModel(TModel model)
    //     {
    //         this._model = model;
    //     }

    //     public abstract Task Create()
    //     {

    //     }
    //     public abstract Task<TModel> Read();
    //     public abstract Task Update();
    //     public abstract Task Delete();
    // }
    //
    // public interface IModel { }
    //
    // public abstract class Model
    // {
    //     protected IDao<Model>? dao;
    //
    //     public int Id { get; set; }
    //
    //     public Model(IDao<Model> dao)
    //     {
    //         this.dao = dao;
    //         this.dao.SetModel(this);
    //     }
    //
    //     public virtual async Task Create(Model model)
    //     {
    //         await dao!.Create();
    //     }
    //
    //     public virtual async Task Delete()
    //     {
    //         await dao!.Delete();
    //     }
    //
    //     public virtual async Task Update()
    //     {
    //         await dao!.Update();
    //     }
    //
    //     public virtual async Task<Model?> Read()
    //     {
    //         return await dao!.Read(this);
    //     }
    //
    //     public virtual async Task<IEnumerable<Model>?> ReadMultiple()
    //     {
    //         return await dao!.ReadMultiple();
    //     }
    //
    // }

}